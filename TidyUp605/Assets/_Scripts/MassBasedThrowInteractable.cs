using System; // Required for Actions
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
public class MassBasedThrowInteractable : XRGrabInteractable
{
    [Header("Mass-Based Grab Settings")]
    public float TwoHandedMassThreshold = 2.0f;

    [Header("Throw Velocity Settings")]
    public float StandardMass = 1.0f;
    public float BaseVelocityScale = 1.25f;
    public float MinVelocityScale = 0.5f;
    public float MaxVelocityScale = 3.0f;

    // --- EVENTS: Other scripts can listen to these! ---
    public event Action OnHeavyObjectLocked;      // Fired when 1 hand fails to lift heavy object
    public event Action OnOneHandedLightGrab;     // Fired when 1 hand lifts light object
    public event Action OnTwoHandedGrab;          // Fired when 2 hands lift heavy object
    public event Action OnFullyDropped;           // Fired when object is dropped completely

    private Rigidbody m_CachedRigidbody;
    private bool m_OriginalIsKinematic;
    private MovementType m_OriginalMovementType;
    private string m_LogPrefix;

    protected override void Awake()
    {
        base.Awake();
        m_LogPrefix = $"[MassBasedGrab ({gameObject.name})] ";

        m_CachedRigidbody = GetComponent<Rigidbody>();
        if (m_CachedRigidbody == null)
        {
            Debug.LogError(m_LogPrefix + "Requires a Rigidbody component.", this);
            return;
        }

        // Capture original state
        m_OriginalIsKinematic = m_CachedRigidbody.isKinematic;
        m_OriginalMovementType = movementType;

        selectMode = InteractableSelectMode.Multiple;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (m_CachedRigidbody == null) { base.ProcessInteractable(updatePhase); return; }

        bool isHeavy = m_CachedRigidbody.mass > TwoHandedMassThreshold;

        if (isHeavy)
        {
            int handCount = interactorsSelecting.Count;

            // Case: Heavy, 1 hand -> LOCK
            if (handCount == 1)
            {
                if (isSelected)
                {
                    if (!m_CachedRigidbody.isKinematic) m_CachedRigidbody.isKinematic = true;
                    return; // Stop movement
                }
                else { m_CachedRigidbody.isKinematic = m_OriginalIsKinematic; }
            }
            // Case: Heavy, 2+ hands -> MOVE
            else if (handCount >= 2)
            {
                if (m_CachedRigidbody.isKinematic) m_CachedRigidbody.isKinematic = m_OriginalIsKinematic;
                base.ProcessInteractable(updatePhase); return;
            }
            // Case: Dropped
            else if (handCount == 0)
            {
                if (m_CachedRigidbody.isKinematic) m_CachedRigidbody.isKinematic = m_OriginalIsKinematic;
                base.ProcessInteractable(updatePhase); return;
            }
        }
        // Case: Light Object
        base.ProcessInteractable(updatePhase);
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (m_CachedRigidbody == null) { base.OnSelectEntering(args); return; }

        bool isHeavy = m_CachedRigidbody.mass > TwoHandedMassThreshold;

        if (isHeavy)
        {
            selectMode = InteractableSelectMode.Multiple;

            if (interactorsSelecting.Count == 0) // First hand grabbing heavy
            {
                movementType = MovementType.Kinematic;
                // NOTIFY LISTENERS: Locked
                OnHeavyObjectLocked?.Invoke();
            }
            else if (interactorsSelecting.Count == 1) // Second hand grabbing heavy
            {
                movementType = m_OriginalMovementType;
                m_CachedRigidbody.isKinematic = m_OriginalIsKinematic;
                // NOTIFY LISTENERS: Two Handed Success
                OnTwoHandedGrab?.Invoke();
            }
        }
        else
        {
            selectMode = InteractableSelectMode.Single;
            movementType = m_OriginalMovementType;
            m_CachedRigidbody.isKinematic = m_OriginalIsKinematic;
            // NOTIFY LISTENERS: Light Grab
            OnOneHandedLightGrab?.Invoke();
        }

        base.OnSelectEntering(args);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        // --- THROW FIX LOGIC ---
        // If this is the last hand leaving, we must restore physics BEFORE the throw calculation
        if (interactorsSelecting.Count == 1)
        {
            movementType = m_OriginalMovementType;
            if (m_CachedRigidbody != null) m_CachedRigidbody.isKinematic = false;
        }

        // Dynamic Throw Velocity Calculation
        if (m_CachedRigidbody != null)
        {
            float massRatio = StandardMass / m_CachedRigidbody.mass;
            float dynamicScale = BaseVelocityScale * massRatio;
            dynamicScale = Mathf.Clamp(dynamicScale, MinVelocityScale, MaxVelocityScale);
            throwVelocityScale = dynamicScale;
        }

        base.OnSelectExiting(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        // Reset states
        selectMode = InteractableSelectMode.Multiple;

        // If no hands left, notify that we are fully dropped
        if (interactorsSelecting.Count == 0)
        {
            OnFullyDropped?.Invoke();
        }

        base.OnSelectExited(args);
    }
}