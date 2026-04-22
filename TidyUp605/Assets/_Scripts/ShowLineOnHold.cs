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
        if (triggerAction != null && curveVisual != null)
        {
            // Check if the trigger is being pulled
            bool isHolding = triggerAction.action.ReadValue<float>() > 0.1f;

            // Turn the XRI Logic on/off
            curveVisual.enabled = isHolding;

            if (lineRenderer != null)
            {
                lineRenderer.enabled = isHolding;
            }
        }
    }
}
