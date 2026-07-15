using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ToggleBothFarCasters : MonoBehaviour
{
    [Header("Interactors")]
    public NearFarInteractor leftInteractor;
    public NearFarInteractor rightInteractor;

    public void ToggleLasers()
    {
        if (leftInteractor != null && rightInteractor != null)
        {
            // Read the current state from the left hand and flip it
            bool newState = !leftInteractor.enableFarCasting;

            // Apply the new state to the enableFarCasting bool on BOTH hands
            leftInteractor.enableFarCasting = newState;
            rightInteractor.enableFarCasting = newState;
        }
    }
    public void SetLaserState (bool IsEnabled)
    {
        if (leftInteractor != null && rightInteractor != null)
        {
            leftInteractor.enableFarCasting = IsEnabled;
            rightInteractor.enableFarCasting = IsEnabled;
        }
    }

    public string GetLaserState()
    {
        string value = $"Laserstate: left = {leftInteractor.enableFarCasting}, right = {rightInteractor.enableFarCasting}.";
        return value;
    }
}
