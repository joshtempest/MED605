using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals;

public class ShowLineOnHold : MonoBehaviour
{
    [Tooltip("The controller button you hold to aim (e.g., Trigger)")]
    public InputActionReference triggerAction;

    [Tooltip("Drag the Curve Visual Controller here")]
    public CurveVisualController curveVisual;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        // If the trigger is pulled more than 10%, turn the line on. Otherwise, hide it.
        if (triggerAction != null && curveVisual != null)
        {
            bool isHolding = triggerAction.action.ReadValue<float>() > 0.1f;

            curveVisual.enabled = isHolding;

            // Force the line renderer invisible when not holding
            if (lineRenderer != null && !isHolding)
            {
                lineRenderer.enabled = false;
            }
        }
    }
}
