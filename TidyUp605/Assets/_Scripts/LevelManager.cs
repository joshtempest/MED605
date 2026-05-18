using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level test settings")]
    [SerializeField] private bool thisIsIntro;
    [SerializeField] private bool thisIsTrial1;

    //how many seconds to wait before playing next level instructions
    [SerializeField] private float audioBuffer = 1.5f;

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

    //To access the spawning of the sorting objects
    private objectSpawner spawnerScript;
    private GameController gameController;

    //to (hopefully) avoid the objects spawning in the floor
    [Header("Spawn Buffers")]
    [SerializeField] Vector3 koeleSpawnbuffer = new Vector3(0, 1.62f, 0);
    [SerializeField] Vector3 skabSpawnbuffer = new Vector3(0, 0.2f, 0);
    [SerializeField] Vector3 opvaskeSpawnbuffer = new Vector3(0, 0.1f, 0);

    Vector3 koelePlatformPos;
    Vector3 skabPlatformPos;
    Vector3 opvaskemaskinePlatformPos;

    //for scene changing
    [Header("numbers xD")]
    public string currentLevel;
    [SerializeField] int currentStage;
    [SerializeField] int currentSequence;
    [SerializeField] bool evalActive = false;
    Scene activeScene;


    [Header("Cross-Scene Memory")]
    public static string pendingLevelToLoad = "";
    public static int pendingSequenceType = 0;

    //dEBUG lOG
    public static int lvlManagerIndex = 0;
    List<LevelManager> LevelManagers = new();

    // The active LevelManager in the current scene.
    // This is intentionally NOT a DontDestroyOnLoad singleton because
    // LevelManager lives on GameObjects that must be unique per scene and should be destroyed.
    public static LevelManager Current { get; private set; }

    private void Awake()
    {
        // register as active for this scene
        Current = this;

        Debug.Log($"[LevelManager Awake] id={GetInstanceID()} thisIsIntro={thisIsIntro} thisIsTrial1={thisIsTrial1} time={Time.frameCount}");
        spawnerScript = this.gameObject.GetComponent<objectSpawner>();
        gameController = this.gameObject.GetComponent<GameController>();

        koelePlatformPos = koelePlatform.transform.position + koeleSpawnbuffer;
        skabPlatformPos = skabPlatform.transform.position + skabSpawnbuffer;
        opvaskemaskinePlatformPos = opvaskemaskinePlatform.transform.position + opvaskeSpawnbuffer;

        if (thisIsIntro) { currentLevel = "Intro"; }
        else if (thisIsTrial1) { currentLevel = "Trial1"; }
    }

    private void OnDestroy()
    {
        // Clear the static reference only if this instance is the registered current.
        if (Current == this)
        {
            Debug.Log($"[LevelManager OnDestroy] Clearing Current (id={GetInstanceID()})");
            Current = null;
        }
    }

    // Static safe helper UI / other code can call to target the active LevelManager in the scene.
    public static void ReloadActiveLevel_Static()
    {
        if (Current != null)
        {
            Current.reloadLevel();
            return;
        }

        // Fallback: try to find a LevelManager in the scene (useful if Current was cleared unexpectedly)
        var found = FindObjectOfType<LevelManager>();
        if (found != null)
        {
            Debug.Log("[LevelManager] ReloadActiveLevel_Static: found LevelManager via FindObjectOfType, invoking reload.");
            found.reloadLevel();
            Current = found;
            return;
        }

        Debug.LogWarning("[LevelManager] ReloadActiveLevel_Static: no active LevelManager found to reload.");
    }

    private void Start()
    {
        Debug.Log($"[LevelManager Start] id={GetInstanceID()} currentLevel='{currentLevel}' pendingLevelToLoad='{pendingLevelToLoad}' pendingSeq={pendingSequenceType} time={Time.frameCount}");
        Debug.Log($"[LevelManager Start] LevelManager count={GameObject.FindGameObjectsWithTag("GameController").Length}");
        
        if (!string.IsNullOrEmpty(pendingLevelToLoad))
        {
            Debug.Log($"Found a pending level! Loading: {pendingLevelToLoad} on id={GetInstanceID()}");

            if (pendingSequenceType == 1)
            {
                ContinueLoadSequence1(pendingLevelToLoad);
            }
            else if (pendingSequenceType == 2)
            {
                ContinueLoadSequence2(pendingLevelToLoad);
            }
            else if (pendingSequenceType == 3)
            {
                // Continue evaluation load on the new LevelManager instance
                ContinueLoadStage1Eval();
            }

            // Clear pending after processing
            pendingLevelToLoad = "";
            pendingSequenceType = 0;
        }
    }

    //housekeeping - disabling click, resetting the blackboard
    public void PrepareLoadNew()
    {
        if (ReviewManager.instance)
        {
            ReviewManager.instance.ClearBoard();
            ReviewManager.instance.DisplayGameScreen();

        }

        //disable laser
        if (GameController.laserManager) GameController.laserManager.SetLaserState(false);
    }

    ///Destroys all objects with the tags "receptacle", "Mad", "Service" and "Beskidt", to clear the scene.
    void annihilation()
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

    ///reloads the current level by checking which level is currently active and loading it again.
    //from here vv      do we still use this?? -G
    public void reloadLevel()
    {
        Debug.Log($"[reloadLevel CALL] id={GetInstanceID()} currentLevel='{currentLevel}' stage={currentStage} seq={currentSequence} pending='{pendingLevelToLoad}' time={Time.frameCount}");
        //Debug.Log("Reloading level: " + currentLevel + " and this is not LevelManager no. " + lvlManagerIndex);//lvlManIndex is currently bork
        //Debug.Log("Current stage: " + currentStage + ", Current sequence: " + currentSequence + ", Eval active: " + evalActive);

        if (currentStage == 1)
        {
            if (currentSequence == 1) { loadSequence1(currentLevel); }
            else if (currentSequence == 2) { loadSequence2(currentLevel); }
            else if (evalActive) { loadStage1Eval(); }
        }
        else if (currentLevel == "Trial1") { loadTrial1(); }
        else if (currentLevel == "Intro") { loadIntro(); }
        else if (currentLevel == "LevelSelect") { loadLevelSelect(); }
        else if (currentLevel == "VR") { SceneManager.LoadScene("VRTutorial"); }
    }
    public void reloadLevel(string levelReload)
    {
        Debug.Log("Reloading level: " + currentLevel + " and this is not LevelManager no. " + lvlManagerIndex);//lvlManIndex is currently bork
        Debug.Log("Current stage: " + currentStage + ", Current sequence: " + currentSequence + ", Eval active: " + evalActive);
        if (currentLevel == null || currentLevel == "")
        {
            currentLevel = levelReload;
        }
        if (currentStage == 1)
        {
            if (currentSequence == 1) { loadSequence1(currentLevel); }
            else if (currentSequence == 2) { loadSequence2(currentLevel); }
            else if (evalActive) { loadStage1Eval(); }
        }

        if (currentLevel == "Trial1") { loadTrial1(); }
        else if (currentLevel == "Intro") { loadIntro(); }
        else if (currentLevel == "LevelSelect") { loadLevelSelect(); }
        else if (currentLevel == "VR") { SceneManager.LoadScene("VRTutorial"); }
    }

    ///reloads the current level after a specified delay, to give the player time to see the feedback before the level resets.
    public void reloadLevel(float delayInSeconds)
    {
        StartCoroutine(waitAndReloadLevel(delayInSeconds));
    }

    public IEnumerator waitAndReloadLevel(float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Reloading level: " + currentLevel);
        ////LogData.instance.AddToLogs("Reloading level: " + currentLevel);

        if (currentStage == 1)
        {
            if (currentSequence == 1) { loadSequence1(currentLevel); }
            else if (currentSequence == 2) { loadSequence2(currentLevel); }
            else if (evalActive) { loadStage1Eval(); }
        }

        if (currentLevel == "Trial1") { loadTrial1(); }
        else if (currentLevel == "Intro") { loadIntro(); }
        else if (currentLevel == "LevelSelect") { loadLevelSelect(); }
        else if (currentLevel == "VR") { loadVR(); }
    }
    //to here ^^

    ///Loads the next level based on the current level, this is used to progress through the levels in a set order. It checks which level is currently active and loads the next one accordingly.
    public void loadNextLevel()
    {
        //LogData.instance.AddToLogs("Loading Next Level after " + currentLevel);

        Debug.Log("Loading next level after: " + currentLevel);
        if (currentLevel == "VR") { loadSequence1("T1"); }
        if (currentStage == 1)
        {
            if (currentSequence == 1)
            {
                if (currentLevel == "T1") { loadSequence1("T2"); }
                else if (currentLevel == "T2") { loadSequence1("P1"); }
                else if (currentLevel == "P1") { loadSequence1("P2"); }
                else if (currentLevel == "P2") { loadSequence1("P3"); }
                else if (currentLevel == "P3") { loadSequence1("P4"); }
                else if (currentLevel == "P4") { loadSequence2("T1"); }
            }
            else if (currentSequence == 2)
            {
                if (currentLevel == "T1") { loadSequence2("T2"); }
                else if (currentLevel == "T2") { loadSequence2("P1"); }
                else if (currentLevel == "P1") { loadSequence2("P2"); }
                else if (currentLevel == "P2") { loadSequence2("P3"); }
                else if (currentLevel == "P3") { loadSequence2("P4"); }
                else if (currentLevel == "P4") { loadStage1Eval(); }
            }
            else if (evalActive) { loadOutro(); }
        }
    }

    ///overload of loadNextLevel to add a delay before loading the next level, to give the player time to see the feedback before progressing to the next level.
    public void loadNextLevel(float delayInSeconds)
    {
        StartCoroutine(waitAndLoadNextLevel(delayInSeconds));
    }
    public IEnumerator waitAndLoadNextLevel(float delay)
    {
        ////LogData.instance.AddToLogs("Loading Next Level after " + currentLevel);

        yield return new WaitForSeconds(delay);

        if (currentStage == 1)
        {
            if (currentSequence == 1)
            {
                if (currentLevel == "T1") { loadSequence1("T2"); }
                else if (currentLevel == "T2") { loadSequence1("P1"); }
                else if (currentLevel == "P1") { loadSequence1("P2"); }
                else if (currentLevel == "P2") { loadSequence1("P3"); }
                else if (currentLevel == "P3") { loadSequence1("P4"); }
                else if (currentLevel == "P4") { loadSequence2("T1"); }
            }
            else if (currentSequence == 2)
            {
                if (currentLevel == "T1") { loadSequence2("T2"); }
                else if (currentLevel == "T2") { loadSequence2("P1"); }
                else if (currentLevel == "P1") { loadSequence2("P2"); }
                else if (currentLevel == "P2") { loadSequence2("P3"); }
                else if (currentLevel == "P3") { loadSequence2("P4"); }
                else if (currentLevel == "P4") { loadStage1Eval(); }
            }
            else if (evalActive) { loadLevelSelect(); }
        }
    }

    ///Makes sure that the needed scene is loaded.
    bool compareScene(string neededScene)
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

    // Coroutine that loads a scene asynchronously and invokes 'onLoaded' after the scene is active

    ///Old system intro, not broken, so still in use
    public void loadIntro()
    {
        currentLevel = "Intro";
        compareScene("Intro");
        annihilation();
        //Debug.Log(currentLevel + " is being loaded");

        //narrator attempt
        AudioManager.Instance.PlaySFXWithDelay("intro", 5f);
    }
    ///Old system trial, not broken, so still in use
    public void loadTrial1()
    {
        currentLevel = "Trial1";
        compareScene("TrialScene");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        gameController.totalThreshold = 3; //G
        //Debug.Log(currentLevel + " is being loaded");
    }
    ///Enables loading of custom levels, based on sequence 1.
    public void loadCustomSeq1(int cleanTallerken, int dirtyTallerken, bool boolskab, bool boolopvask, bool boolkoele, int threshold)
    {
        compareScene("Tutorial_Practice");
        annihilation();

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

    ///Enables loading of Sequence 1 levels, based on the level name.
    public void loadSequence1(string levelName)
    {
        PrepareLoadNew();

        pendingLevelToLoad = levelName;
        pendingSequenceType = 1;

        if (compareScene("Tutorial_Practice")) { return; }

        pendingLevelToLoad = "";
        pendingSequenceType = 0;
        ContinueLoadSequence1(levelName);
    }
    private void ContinueLoadSequence1(string levelName)
    {
        //Debug.Log("Scene loaded, now loading level content for: " + levelName);

        ///Made to enable more stages and sequences in the future. 
        Debug.Log($"[ContinueLoadSequence1 ENTER] id={GetInstanceID()} levelName='{levelName}' time={Time.frameCount}");
        currentStage = 1;
        currentSequence = 1;
        ///Calls annihilation to clear the scene
        annihilation();
        ///Checks which level is being loaded and instantiates the needed objects and sets the right-threshold accordingly.
        if (levelName == "T1")
        {
            ///set current level, to enable reloading of the same level and loading the next level in the correct order
            currentLevel = levelName;
            Debug.Log($"[ContinueLoadSequence1] set currentLevel='{currentLevel}' on id={GetInstanceID()}");
            //Debug.Log("current level: " + currentLevel);
            //Debug.Log("level name: " + levelName);
            ///Instantiate the needed receptacles for this level, based on a platform made in the scene.
            Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
            ///Spawn one object for sorting as a starting point
            spawnerScript.spawnThisObject("rT");
            ///Sets how many answers are needed before moving on
            gameController.totalThreshold = 1;
        }
        else if (levelName == "T2")
        {
            currentLevel = levelName;
            Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
            spawnerScript.spawnThisObject("bT");
            gameController.totalThreshold = 1;
        }
        else if (levelName == "P1")
        {
            currentLevel = levelName;

            Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
            Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
            gameController.totalThreshold = 2;
            ///Set the amount of clean and dirty plates to spawn, based on the level, and spawn them using the spawner script.
            int cleanTallerken = 1;
            int dirtyTallerken = 1;
            for (int i = 0; i < cleanTallerken; i++)
            {
                spawnerScript.spawnThisObject("rT");
            }
            for (int i = 0; i < dirtyTallerken; i++)
            {
                spawnerScript.spawnThisObject("bT");
            }
        }
        else if (levelName == "P2")
        {
            currentLevel = levelName;

            Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
            Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
            gameController.totalThreshold = 3;
            int cleanTallerken = 1;
            int dirtyTallerken = 2;
            for (int i = 0; i < cleanTallerken; i++)
            {
                spawnerScript.spawnThisObject("rT");
            }
            for (int i = 0; i < dirtyTallerken; i++)
            {
                spawnerScript.spawnThisObject("bT");
            }
        }
        else if (levelName == "P3")
        {
            currentLevel = levelName;

            Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
            Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
            gameController.totalThreshold = 4;
            int cleanTallerken = 2;
            int dirtyTallerken = 2;
            for (int i = 0; i < cleanTallerken; i++)
            {
                spawnerScript.spawnThisObject("rT");
            }
            for (int i = 0; i < dirtyTallerken; i++)
            {
                spawnerScript.spawnThisObject("bT");
            }
        }
        else if (levelName == "P4")
        {
            currentLevel = levelName;

            Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
            Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
            gameController.totalThreshold = 5;
            int cleanTallerken = 1;
            int dirtyTallerken = 4;
            for (int i = 0; i < cleanTallerken; i++)
            {
                spawnerScript.spawnThisObject("rT");
            }
            for (int i = 0; i < dirtyTallerken; i++)
            {
                spawnerScript.spawnThisObject("bT");
            }
        }
        else { Debug.Log(levelName + " is invalid"); }


        //play instructions according to objects in scene (hasPlates / hasForks); all objects in sequence1 are plates
        AudioManager.Instance.PlayPracticeInstructions(audioBuffer, true, false);
        Debug.Log($"[ContinueLoadSequence1 EXIT] id={GetInstanceID()} currentLevel='{currentLevel}' stage={currentStage} seq={currentSequence}");
    }
    ///Does the same thing as loadSequence1, but with other gameObjects.
    public void loadSequence2(string levelName)
    {
        PrepareLoadNew();

        pendingLevelToLoad = levelName;
        pendingSequenceType = 2;

        if (compareScene("Tutorial_Practice")) { return; }

        pendingLevelToLoad = "";
        pendingSequenceType = 0;
        ContinueLoadSequence2(levelName);
    }
    private void ContinueLoadSequence2(string levelName)
    {
        //Debug.Log("Scene loaded, now loading level content for: " + levelName);
        currentStage = 1;
        currentSequence = 2;
        annihilation();

        if (levelName == "T1")
        {
            currentLevel = levelName;
            Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
            spawnerScript.spawnThisObject("rG");
            gameController.totalThreshold = 1;
        }
        else if (levelName == "T2")
        {
            currentLevel = levelName;
            Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
            spawnerScript.spawnThisObject("bG");
            gameController.totalThreshold = 1;
        }
        else if (levelName == "P1")
        {
            currentLevel = levelName;

            Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
            Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
            gameController.totalThreshold = 2;
            int cleanTallerken = 1;
            int dirtyTallerken = 1;
            for (int i = 0; i < cleanTallerken; i++)
            {
                spawnerScript.spawnThisObject("rG");
            }
            for (int i = 0; i < dirtyTallerken; i++)
            {
                spawnerScript.spawnThisObject("bG");
            }
        }
        else if (levelName == "P2")
        {
            currentLevel = levelName;

            Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
            Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
            gameController.totalThreshold = 3;
            int cleanTallerken = 1;
            int dirtyTallerken = 2;
            for (int i = 0; i < cleanTallerken; i++)
            {
                spawnerScript.spawnThisObject("rG");
            }
            for (int i = 0; i < dirtyTallerken; i++)
            {
                spawnerScript.spawnThisObject("bG");
            }
        }
        else if (levelName == "P3")
        {
            currentLevel = levelName;

            Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
            Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
            gameController.totalThreshold = 4;
            int cleanTallerken = 2;
            int dirtyTallerken = 2;
            for (int i = 0; i < cleanTallerken; i++)
            {
                spawnerScript.spawnThisObject("rG");
            }
            for (int i = 0; i < dirtyTallerken; i++)
            {
                spawnerScript.spawnThisObject("bG");
            }
        }
        else if (levelName == "P4")
        {
            currentLevel = levelName;

            Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
            Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
            gameController.totalThreshold = 5;
            int cleanTallerken = 1;
            int dirtyTallerken = 4;
            for (int i = 0; i < cleanTallerken; i++)
            {
                spawnerScript.spawnThisObject("rG");
            }
            for (int i = 0; i < dirtyTallerken; i++)
            {
                spawnerScript.spawnThisObject("bG");
            }
        }
        else { Debug.LogWarning(levelName + " is invalid"); }

        //Initialise the Blackboard & disable laser
        PrepareLoadNew();


        //play instructions according to objects in scene (hasPlates / hasForks); all objects in sequence1 are plates
        AudioManager.Instance.PlayPracticeInstructions(audioBuffer, false, true);
    }
    ///Load the evaluation scene for stage 1, with a set amount of objects and a higher threshold, to evaluate the player's understanding of the sorting task.
    public void loadStage1Eval()
    {
        currentStage = 1;
        currentSequence = 3;
        evalActive = true;

        pendingLevelToLoad = "S1E";
        pendingSequenceType = 3;

        if (compareScene("Evaluation")) { return; }

        pendingLevelToLoad = "";
        pendingSequenceType = 0;
        ContinueLoadStage1Eval();
    }
    // New helper — the spawn/initialization logic that must run after the Evaluation scene is active.
    private void ContinueLoadStage1Eval()
    {
        currentLevel = "S1E";
        gameController.totalThreshold = 8;

        for (int i = 0; i < 2; i++)
        {
            spawnerScript.spawnThisObject("rT");
            spawnerScript.spawnThisObject("bT");
            spawnerScript.spawnThisObject("rG");
            spawnerScript.spawnThisObject("bG");
            //Debug.Log("Spawning objects, iteration: " + i);
        }

        // Initialise the Blackboard & disable laser
        PrepareLoadNew();
        gameController.resetScore();


        //play instructions according to objects in scene (hasPlates / hasForks); all objects in sequence1 are plates
        AudioManager.Instance.PlayPracticeInstructions(audioBuffer, true, true);
    }

    ///Loads the level select scene, where the player can choose which level to play.
    ///loadLevelSelect and loadVR is part of the old system, but they did not need to be updated to function.
    public void loadLevelSelect()
    {
        currentLevel = "LevelSelect";
        compareScene("LevelSelect");
        //Debug.Log(currentLevel + " is being loaded");
        ////LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
    public void loadVR()
    {
        PrepareLoadNew();
        pendingLevelToLoad = "VR";
        if (compareScene("VRTutorial")){ return; }
        pendingLevelToLoad = "";
        continueLoadVR();
    }
    private void continueLoadVR() 
    {
        currentLevel = "VR";
        //gameController.resetScore();
        Debug.Log(currentLevel + " is being loaded");
        ////LogData.instance.AddToLogs(currentLevel + " is being loaded");
        //gameController.totalThreshold = 2;

        //Initialise the Blackboard & disable laser
        

        //narrator attempt; adjust delays
        //introduction
        AudioManager.Instance.PlaySFXWithDelay("v1", audioBuffer);
        AudioManager.Instance.PlaySFXWithDelay("v2", audioBuffer + 2f);

        //instructions - function also used by a button; play after the other two (=added delay) (adjust delay)
        AudioManager.Instance.PlayVRInstructions(audioBuffer + 4f);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void loadOutro()
    {
        compareScene("Outro_new");
        currentLevel = "Outro";
        AudioManager.Instance.PlaySFXWithDelay("outro1", audioBuffer);
        AudioManager.Instance.PlaySFXWithDelay("outro2", audioBuffer + 2f);

    }

}