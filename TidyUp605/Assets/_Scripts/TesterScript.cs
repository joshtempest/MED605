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
        if (oldSys)
        {
            levelManager.currentLevel = sceneToTest;
            levelManager.reloadLevel();
            Debug.Log("Testing scene: " + sceneToTest);
        }
        if (newSys) 
        {
            if (sceneToTest == "Eval")
            {
                levelManager.loadStage1Eval();
            }
            else { levelManager.loadSequence1(sceneToTest); }
        }
    }
}
