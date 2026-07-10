using System;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.SceneManagement;


//This whole class was not visible before Sytem.Serializable was added, but the Tuple is still not visible.
[System.Serializable]
public class PrefabData
{
    public string objectsToInstantiate2;
    public GameObject objectToSpawn;
    public int amountToSpawn;
}

[Serializable]
public class LevelData
{
    public string name;
    
    public LevelType levelType;
    
    public AudioClip[] narratorVoicelines;
    
    //public Tuple<string, int>[] objectsToInstantiate;
    //got a tuple-like to show, as Unity doesn't support tuples in the inspector, as far as I can tell.
    public PrefabData[] objectsToInstantiate2;

    //public GameObject spawnPlatform;

    //spawnBuffer is handled in objectSpawner, for items. For the receptacles they are preset to ensure they spawn on the floor, instead of floating over, or in the floor.
    //public float spawnBuffer;

    public bool boolskab;
    public bool boolopvask;

    //is this not easier to read if named threshold? or thresholdToSet?
    public int numberOfObjects;

}


//

public enum LevelType
{
    VRTutorial,
    Practice,
    Evaluation
}

//

public class NewLevelManager : MonoBehaviour
{
    [Header("Level Data")]
    public LevelData[] Levels;
    public LevelData Evaluation1;

    public static NewLevelManager instance;

    //Prefab
    [Header("Prefabs")]
    //[SerializeField] GameObject koeleskab;
    [SerializeField] GameObject skab;
    [SerializeField] GameObject opvaskemaskine;

    /*
    //Platforms (to place them the same place everytime)
    [Header("Platforms")]
    [SerializeField] GameObject koelePlatform;
    [SerializeField] GameObject skabPlatform;
    [SerializeField] GameObject opvaskemaskinePlatform;

    //to (hopefully) avoid the objects spawning in the floor
    [Header("Spawn Buffers")]
    [SerializeField] Vector3 koeleSpawnbuffer = new Vector3(0, 1.62f, 0);
    [SerializeField] Vector3 skabSpawnbuffer = new Vector3(0, 0.2f, 0);
    [SerializeField] Vector3 opvaskeSpawnbuffer = new Vector3(0, 0.1f, 0);
    */

    public Vector3 skabRotation;
    public Vector3 skabPosition;
    public Vector3 opvaskerRotation;
    public Vector3 opvaskemaskinePosition;

    public Vector3 blackboardRotation;
    public Vector3 blackboardPosition;
    public Vector3 evalBBoffset = new Vector3(-5f, 0f, 0f);

    //to make levels possible when starting from LevelSelect
    public GameObject audioManagerPrefab;
    //public GameObject reviewManagerPrefab;
    public GameObject blackboardPrefab;



    //To access the spawning of the sorting objects
    private objectSpawner spawnerScript;
    private GameController gameController;
    private AudioManager audioManager;
    public GameObject blackboard;
    private ReviewManager reviewManager;

    public GameObject spawnPlatform;

    Scene activeScene;

    public string currentLevel;

    public LevelData currentLevelData;

    string requestedLevel;

    bool TPLoaded = false;
    bool VRLoaded = false;
    bool EVALLoaded = false;

    //Debug only
    private void Update()
    {
        if (TPLoaded)
        {
            TPLoaded = false;
            LoadCustomSequence(requestedLevel);
        }

        if (EVALLoaded)
        {
            EVALLoaded = false;
            LoadCustomSequence(requestedLevel);
        }
    }


    //singleton pattern
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        spawnerScript = this.gameObject.GetComponent<objectSpawner>();
        gameController = this.gameObject.GetComponent<GameController>();
        spawnPlatform = GameObject.FindGameObjectWithTag("SpawnPlatform");

    }


    //Makes sure that the needed scene is loaded.
    public bool CompareScene(string neededScene)
    {
        //Debug.Log("comparing scene");
        activeScene = SceneManager.GetActiveScene();
        string sceneName = activeScene.name;
        if (sceneName != neededScene)
        {
            SceneManager.LoadScene(neededScene);
            return true;
        }
        return false;
    }



    void Annihilation()
    {
        Debug.Log("Annihilation initiated.");
        //LogData.instance.AddToLogs("Annihalating...");

        ///Finds all objects with the specified tags and destroys them, to clear the scene for the new level.
        ///Receptacles need to be cleared, to insure that only the needed receptacles for the level are present.
        GameObject[] receptables = GameObject.FindGameObjectsWithTag("receptacle");
        for (int i = 0; i < receptables.Length; i++)
        {
            Destroy(receptables[i]);
        }
        GameObject[] mad = GameObject.FindGameObjectsWithTag("Mad");
        for (int i = 0; i < mad.Length; i++)
        {
            Destroy(mad[i]);
        }
        GameObject[] service = GameObject.FindGameObjectsWithTag("Service");
        for (int i = 0; i < service.Length; i++)
        {
            Destroy(service[i]);
        }
        GameObject[] beskidt = GameObject.FindGameObjectsWithTag("Beskidt");
        for (int i = 0; i < beskidt.Length; i++)
        {
            Destroy(beskidt[i]);
        }

    }

    public void LoadCustomSequence(string levelName)
    {
        Debug.Log($"Attempting to load level {levelName}");

        //make sure we are in TP scene
        //CompareScene("Tutorial_Practice");

        Debug.Log("Annihalating...");

        Annihilation();

        ReviewManager.instance.DisplayGameScreen();

        LevelData levelToLoad = null;

        Debug.Log("Identifying LevelToLoad...");
        //iterate through LevelData[] to find the right data
        foreach (LevelData l in Levels)
        {
            if (l.name == levelName)
            {
                levelToLoad = l;
            }
        }
        if (levelName == "Eval1")
        {
            levelToLoad = Evaluation1;
            Debug.Log("Loading Evaluation...");
        }
        //if no data was found: abort
        if (levelToLoad == null)
        {
            Debug.LogWarning($"Unable to load level from string {levelName}. Please enter a valid level identifier and try again.");
            return;
        }
        Debug.Log($"LevelToLoad identified: {levelToLoad.name}.");

        currentLevel = levelToLoad.name;
        currentLevelData = levelToLoad;

        InstantiateInfrastructure(levelToLoad.levelType);

        //reset score when Blackboard is identified (hopefully)
        Debug.Log("Attempting to reset score via gamecontroller...");
        if (gameController != null)
        {
            gameController.resetScore();
        }
        else
        {
            Debug.LogWarning("GameController is missing on " + gameObject.name + ". Cannot reset score.");
        }


        //instantiate receptacles
        Debug.Log($"Instantiating receptacles in scene {SceneManager.GetActiveScene().name}...");
        GameObject GOskab = null;
        GameObject GOopvasker = null;
        if (levelToLoad.boolskab) { GOskab = Instantiate(skab, skabPosition, Quaternion.Euler(skabRotation)); }
        if (levelToLoad.boolopvask) { GOopvasker = Instantiate(opvaskemaskine, opvaskemaskinePosition, Quaternion.Euler(opvaskerRotation)); }
        Debug.Log($"Receptacles instantiated: Skab - {levelToLoad.boolskab} - {GOskab} // Opvasker - {levelToLoad.boolopvask} - {GOopvasker}.");


        Debug.Log($"Instantiating objects: {levelToLoad.numberOfObjects} need to be spawned...");
        foreach(PrefabData p in levelToLoad.objectsToInstantiate2)
        {
            Debug.Log($"Attempting to spawn {p.objectToSpawn} objects...");
            for (int i = 0; i < p.amountToSpawn; i++)
            {
                spawnerScript.SpawnThisObject2(p.objectToSpawn);
            }
        }

        gameController.totalThreshold = levelToLoad.numberOfObjects;

        audioManager.PlayClips(levelToLoad.narratorVoicelines);

        Debug.Log($"Successfully loaded level {levelName}.");
    }

    public void InstantiateInfrastructure(LevelType levelType)
    {
        Debug.Log("Instantiating infrastructure...");
        //try to access components from NewLevelManager prefab
        spawnerScript = this.gameObject.GetComponent<objectSpawner>();
        gameController = this.gameObject.GetComponent<GameController>();
        spawnPlatform = GameObject.FindGameObjectWithTag("SpawnPlatform");

        if (!spawnerScript)
        {
            Debug.LogWarning($"spawnerScript component missing on NewLevelManager prefab, please add.");
        }
        if (!gameController)
        {
            Debug.LogWarning($"gameController component missing on NewLevelManager prefab, please add.");
        }

        //instantiate what we need for a typical game scene
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (!audioManager)
        {
            Instantiate(audioManagerPrefab);
        }
        blackboard = GameObject.Find("Blackboard");
        if (!blackboard)
        {
            blackboard = Instantiate(blackboardPrefab, blackboardPosition, Quaternion.Euler(blackboardRotation));
            reviewManager = blackboard.GetComponent<ReviewManager>();
        }

        if (SceneManager.GetActiveScene().name == "Tutorial_Practice")
        {
            blackboard.transform.position = blackboardPosition;
            blackboard.transform.rotation = Quaternion.Euler(blackboardRotation);
        }
        else if (SceneManager.GetActiveScene().name == "Evaluation")
        {
            blackboard.transform.position = blackboardPosition + evalBBoffset;
            blackboard.transform.rotation = Quaternion.Euler(blackboardRotation);
        }
        else
        {
            blackboard.transform.position = blackboardPosition + new Vector3(0f, -100f, 0f);
            blackboard.transform.rotation = Quaternion.Euler(blackboardRotation);

        }

        if (!reviewManager || !audioManager)
        {
            Debug.LogWarning($"Infrastructure missing: reviewManager = {reviewManager} AudioManager = {audioManager}.");
        }
    }

    public void PlayInstructions()
    {
        audioManager.PlayClips(currentLevelData.narratorVoicelines);
    }
    public void LoadNextLevel()
    {
        Debug.Log($"LoadingNextLevel... Currentlevel: {currentLevel}");
        bool currentLevelFound = false;

        foreach (LevelData l in Levels)
        {
            Debug.Log($"l.name == {l.name}, comparing...");
            if (currentLevelFound)
            {
                Debug.Log($"Loading Next Level: {l.name}, coming after {currentLevel}.");
                LoadCustomSequence(l.name);
                return;
            }
            else if (currentLevel == l.name)
            {
                Debug.Log($"currentLevel identified: {currentLevel} == {l.name}.");
                currentLevelFound = true;
            }
        }

        if (SceneManager.GetActiveScene().name == "Tutorial_Practice")
        {
            LoadEvaluation("Eval1");
        }
        else if (SceneManager.GetActiveScene().name == "Evaluation")
        {
            LoadOutro();
        }
        else
        {
            Debug.LogWarning($"Buddy you tried to load the level after {currentLevel} but you are in {SceneManager.GetActiveScene().name}. This should be physically impossible and I'm only writing this long-ass warning because technically it should never occur. Congratulations! You have expanded the bounds of what is possible. Now go fix yo shit.");
        }

    }

    public void ReloadLevel()
    {
        LoadCustomSequence(currentLevel);
    }

    public void LoadPractice()
    {
        requestedLevel = "1T1";
        CompareScene("Tutorial_Practice");
        
    }

    public void LoadPractice(string name)
    {
        requestedLevel = name;
        CompareScene("Tutorial_Practice");
    }

    public void LoadVRTraining()
    {
        CompareScene("VRTutorial");
        InstantiateInfrastructure(LevelType.VRTutorial);
    }
    public void LoadEvaluation()
    {
        requestedLevel = "Eval1";
        CompareScene("Evaluation");
       
    }
    public void LoadEvaluation(string evalName)
    {

        requestedLevel = evalName;
        CompareScene("Evaluation");


        //InstantiateInfrastructure(LevelType.Evaluation);
    }

    public void LoadOutro()
    {
        CompareScene("Outro_new");
    }

    public void LoadLevelSelect()
    {
        CompareScene("LevelSelect");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Tutorial_Practice")
        {
            TPLoaded = true;
        }
        else if (scene.name == "VRTutorial")
        {
            VRLoaded = true;
        }
        else if (scene.name == "Evaluation")
        {
            EVALLoaded = true;
        }
        Debug.Log($"Scene {SceneManager.GetActiveScene().name} loaded.");
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
