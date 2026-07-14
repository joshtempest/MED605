using UnityEngine;

public class LvlSelect : MonoBehaviour
{
    public void LoadLevel(string name)
    {
        NewLevelManager.instance.LoadPractice(name);
    }

    public void Intro()
    {
        NewLevelManager.instance.LoadIntro();
    }

    public void VRTraining()
    {
        NewLevelManager.instance.LoadVRTraining();
    }

    public void Eval()
    {
        NewLevelManager.instance.LoadEvaluation();
    }

    public void Exit()
    {
        NewLevelManager.instance.LoadOutro();
    }

    public void QuittenGelee()
    {
        Application.Quit();
    }

    public void SwitchToGrab()
    {
        NewLevelManager.instance.LoadGrabVRTraining();
    }

    public void VRNarrator()
    {
        Debug.Log("Trying to play VR narrator...");

        AudioManager.Instance.PlayVRInstructions(0);
    }
}
