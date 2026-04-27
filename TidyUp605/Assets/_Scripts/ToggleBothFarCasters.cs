using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ToggleBothFarCasters : MonoBehaviour
{
    [Header("Interactors")]
    [Tooltip("Drag your Left Near-Far Interactor here")]
    public NearFarInteractor leftInteractor;

    [Tooltip("Drag your Right Near-Far Interactor here")]
    public NearFarInteractor rightInteractor;

    // We track the state internally so the hands never get out of sync
    private bool areLasersEnabled = true;

    void Start()
    {
        // Make sure our internal state matches whatever you set in the Inspector
        if (leftInteractor != null)
        {
            areLasersEnabled = leftInteractor.enableFarCasting;
        }
    }

    // --- Public Methods you can call from UI buttons or other scripts ---

    public void ToggleBothLasers()
    {
        areLasersEnabled = !areLasersEnabled;

        if (leftInteractor != null) leftInteractor.enableFarCasting = areLasersEnabled;
        if (rightInteractor != null) rightInteractor.enableFarCasting = areLasersEnabled;
    }

    public void EnableBothLasers()
    {
        areLasersEnabled = true;
        if (leftInteractor != null) leftInteractor.enableFarCasting = true;
        if (rightInteractor != null) rightInteractor.enableFarCasting = true;
    }

    public void DisableBothLasers()
    {
        areLasersEnabled = false;
        if (leftInteractor != null) leftInteractor.enableFarCasting = false;
        if (rightInteractor != null) rightInteractor.enableFarCasting = false;
    }
}
