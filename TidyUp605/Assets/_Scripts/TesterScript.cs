using System;
using Unity.VisualScripting;
using UnityEngine;

public class TesterScript : MonoBehaviour
{
    LevelManager levelManager;
    [Header("System Select")]
    [SerializeField] private bool oldSys;
    [SerializeField] private bool newSys;

    [Header("Name of scene")]
    [SerializeField] string sceneToTest;

    private void Awake()
    {
        levelManager = this.gameObject.GetComponent<LevelManager>();
    }
    ///Loads/reloads the specified scene, this was made for testing purposes
    private void Start()
    {
        Debug.LogWarning("TesterScript is active.");
        ///We have changed the way levels are loaded, so we need to check which system is active and load the level accordingly
        if (oldSys)
        {
            levelManager.currentLevel = sceneToTest;
            levelManager.reloadLevel();
            //Debug.Log("Testing scene: " + sceneToTest);
        }
        if (newSys) 
        {
            if (sceneToTest == "S1E")
            {
                levelManager.loadStage1Eval();
            }
            else { levelManager.loadSequence1(sceneToTest); }
        }
    }
}
