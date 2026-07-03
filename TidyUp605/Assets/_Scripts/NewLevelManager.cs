using System;
using UnityEngine;

public class LevelData
{
    public string name;
    
    public LevelType levelType;
    
    public AudioClip narratorVoiceline;
    
    public Tuple<string, int>[] objectsToInstantiate;
    
    public GameObject spawnPlatform;
    public float spawnBuffer;

    public bool boolskab;
    public bool boolopvask;

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
    public LevelData[] Levels;

    public static NewLevelManager instance;

    //Prefab
    [Header("Prefabs")]
    [SerializeField] GameObject koeleskab;
    [SerializeField] GameObject skab;
    [SerializeField] GameObject opvaskemaskine;

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

    Vector3 koelePlatformPos;
    Vector3 skabPlatformPos;
    Vector3 opvaskemaskinePlatformPos;


    //to make levels possible when starting from LevelSelect
    public GameObject audioManagerPrefab;
    public GameObject reviewManagerPrefab;
    public GameObject blackboardPrefab;

    //To access the spawning of the sorting objects
    private objectSpawner spawnerScript;
    private GameController gameController;
    private AudioManager audioManager;
    private ReviewManager blackboard;

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

        /*
        koelePlatformPos = koelePlatform.transform.position + koeleSpawnbuffer;
        skabPlatformPos = skabPlatform.transform.position + skabSpawnbuffer;
        opvaskemaskinePlatformPos = opvaskemaskinePlatform.transform.position + opvaskeSpawnbuffer;
        */
    }




    void Annihilation()
    {
        //Debug.Log("Annihilation initiated");
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

        if (gameController != null)
        {
            gameController.resetScore();
        }
        else
        {
            Debug.LogWarning("GameController is missing on " + gameObject.name + ". Cannot reset score.");
        }
    }

    /*
    public void loadCustomSeq1(int cleanTallerken, int dirtyTallerken, bool boolskab, bool boolopvask, bool boolkoele, int threshold)
    {
        //compareScene("Tutorial_Practice");
        Annihilation();

        if (boolskab) { Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation); }
        if (boolopvask) { Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation); }
        if (boolkoele) { Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation); }

        for (int i = 0; i < cleanTallerken; i++)
        {
            spawnerScript.spawnThisObject("rT");
        }
        for (int i = 0; i < dirtyTallerken; i++)
        {
            spawnerScript.spawnThisObject("bT");
        }
        gameController.totalThreshold = threshold;
    }
    */

    public void LoadCustomSequence(string levelName)
    {
        Annihilation();

        LevelData levelToLoad = null;

        //iterate through LevelData[] to find the right data
        foreach (LevelData l in Levels)
        {
            if (l.name == levelName)
            {
                levelToLoad = l;
            }
        }
        //if no data was found: abort
        if (levelToLoad == null)
        {
            Debug.LogWarning($"Unable to load level from string {levelName}. Please enter a valid level identifier and try again.");
            return;
        }

        InstantiateInfrastructure(levelToLoad.levelType);

        //instantiate receptacles
        if (levelToLoad.boolskab) { Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation); }
        if (levelToLoad.boolopvask) { Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation); }
        
        foreach (Tuple<string, int> t in levelToLoad.objectsToInstantiate)
        {
            for (int i = 0; i < t.Item2; i++)
            {
                spawnerScript.spawnThisObject(t.Item1);
            }
        }

        gameController.totalThreshold = levelToLoad.numberOfObjects;

    }

    public void InstantiateInfrastructure(LevelType levelType)
    {
        //try to access components from NewLevelManager prefab
        spawnerScript = this.gameObject.GetComponent<objectSpawner>();
        gameController = this.gameObject.GetComponent<GameController>();
        if (!spawnerScript)
        {
            Debug.LogWarning($"spawnerScript component missing on NewLevelManager prefab, please add.");
        }
        if (!gameController)
        {
            Debug.LogWarning($"gameController component missing on NewLevelManager prefab, please add.");
        }

        //instantiate
        switch (levelType)
            {
                case LevelType.VRTutorial:
                    //instantiate what we need for VR training
                    audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
                    if (!audioManager)
                    {
                        Instantiate(audioManagerPrefab);
                    }
                    blackboard = GameObject.Find("Blackboard").GetComponent<ReviewManager>();
                    if (!blackboard)
                    {
                        Instantiate(blackboardPrefab);    
                    }
                    break;
                case LevelType.Practice:
                    //instantiate what we need for Practice
                    audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
                    if (!audioManager)
                    {
                        Instantiate(audioManagerPrefab);
                    }
                    blackboard = GameObject.Find("Blackboard").GetComponent<ReviewManager>();
                    if (!blackboard)
                    {
                        Instantiate(blackboardPrefab);    
                    }
                    break;
                case LevelType.Evaluation:
                //instantiate what we need for Evaluation
                Debug.LogWarning("Evaluation infrastructure hasn't been coded yet, please add.");
                    break;
            }

    }

}
