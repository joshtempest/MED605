using System;
using Unity.VisualScripting;
using UnityEngine;

public class TesterScript : MonoBehaviour
{
    [SerializeField] string sceneToTest;

    LevelManager levelManager;

    [SerializeField] private bool oldSys;
    [SerializeField] private bool newSys;

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
        if (newSys) { levelManager.loadSequence1(sceneToTest); }
    }
}
