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
    private void Start()
    {
        Debug.LogWarning("TesterScript is active.");// Make sure to disable this script before building the game, otherwise the game will load the scene specified in the TesterScript.");
        if (oldSys)
        {
            levelManager.currentLevel = sceneToTest;
            levelManager.reloadLevel();
            //Debug.Log("Testing scene: " + sceneToTest);
        }
        if (newSys) 
        {
            //Debug.Log("Testing scene: " + sceneToTest);
            if (sceneToTest == "S1E")
            {
                levelManager.loadStage1Eval();
                //Debug.Log("load eval");
            }
            else { levelManager.loadSequence1(sceneToTest); }
        }
    }
}
