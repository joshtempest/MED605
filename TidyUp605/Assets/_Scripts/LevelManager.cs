using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level test settings")]
    [SerializeField] private bool thisIsIntro;
    [SerializeField] private bool thisIsTrial1;

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


    private void Awake()
    {
        spawnerScript = this.gameObject.GetComponent<objectSpawner>();
        gameController = this.gameObject.GetComponent<GameController>();

        koelePlatformPos = koelePlatform.transform.position + koeleSpawnbuffer;
        skabPlatformPos = skabPlatform.transform.position + skabSpawnbuffer;
        opvaskemaskinePlatformPos = opvaskemaskinePlatform.transform.position + opvaskeSpawnbuffer;

        if (thisIsIntro) { currentLevel = "Intro"; }
        else if (thisIsTrial1) { currentLevel = "Trial1"; }
    }

    void annihilation()
    {
        Debug.Log("Annihilation initiated");
        //////LogData.instance.AddToLogs("Annihalating...");
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
       gameController.resetScore();
    }


    public void reloadLevel()
    {
        Debug.Log("Reloading level: " + currentLevel);
        //////LogData.instance.AddToLogs("Reloading level: " + currentLevel);

        if (currentStage == 1)
        {
            if (currentSequence == 1) { loadSequence1(currentLevel); }
            else if (currentSequence == 2) { loadSequence2(currentLevel); }
            else if (evalActive) { loadStage1Eval(); }
        }

        if (currentLevel == "Trial1") { loadTrial1(); }
        /*
        else if (currentLevel == "Tutorial1") {loadTutorial1(); }
        else if (currentLevel == "Tutorial2") {loadTutorial2(); }
        else if (currentLevel == "Tutorial3") {loadTutorial3(); }
        else if (currentLevel == "Practice1") {loadPractice1(); }
        else if (currentLevel == "Practice2") {loadPractice2(); }
        else if (currentLevel == "Practice3") {loadPractice3(); }
        else if (currentLevel == "Practice4") {loadPractice4(); }
        else if (currentLevel == "Practice5") {loadPractice5(); }
        else if (currentLevel == "Practice6") {loadPractice6(); }
        else if (currentLevel == "Practice7") {loadPractice7(); }
        else if (currentLevel == "Eval1") { loadEval1(); }
        */
        else if (currentLevel == "Intro") { loadIntro(); }
        else if (currentLevel == "LevelSelect") { loadLevelSelect(); }
        else if (currentLevel == "VR") { loadVR(); }
    }

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
        /*
        else if (currentLevel == "Tutorial1") { loadTutorial1(); }
        else if (currentLevel == "Tutorial2") { loadTutorial2(); }
        else if (currentLevel == "Tutorial3") { loadTutorial3(); }
        else if (currentLevel == "Practice1") { loadPractice1(); }
        else if (currentLevel == "Practice2") { loadPractice2(); }
        else if (currentLevel == "Practice3") { loadPractice3(); }
        else if (currentLevel == "Practice4") { loadPractice4(); }
        else if (currentLevel == "Practice5") { loadPractice5(); }
        else if (currentLevel == "Practice6") { loadPractice6(); }
        else if (currentLevel == "Practice7") { loadPractice7(); }
        else if (currentLevel == "Eval1") { loadEval1(); }
        */
    }

    public void loadNextLevel()
    {
        ////LogData.instance.AddToLogs("Loading Next Level after " + currentLevel);
        /*
        if (currentLevel == "Intro") { loadTutorial1(); }
        else if (currentLevel == "Tutorial1") { loadTutorial2(); }
        else if (currentLevel == "Tutorial2") { loadTutorial3(); }
        else if (currentLevel == "Tutorial3") { loadPractice1(); }
        else if (currentLevel == "Practice1") { loadPractice2(); }
        else if (currentLevel == "Practice2") { loadPractice3(); }
        else if (currentLevel == "Practice3") { loadPractice4(); }
        else if (currentLevel == "Practice4") { loadPractice5(); }
        else if (currentLevel == "Practice5") { loadPractice6(); }
        else if (currentLevel == "Practice6") { loadPractice7(); }
        else if (currentLevel == "Practice7") { loadEval1(); }
        */ 
        if (currentStage == 1)
        {
            if (currentSequence == 1) 
            { 
                if (currentLevel == "T1") { loadSequence1("T2"); }
                else if (currentLevel == "P1") { loadSequence1("P2"); }
                else if (currentLevel == "P2") { loadSequence1("P3"); }
                else if (currentLevel == "P3") { loadSequence1("P4"); }
                else if (currentLevel == "P4") { loadSequence2("T1"); } 
            }
            else if (currentSequence == 2) 
            {
                if(currentLevel == "T1") { loadSequence2("T2"); }
                    else if (currentLevel == "P1") { loadSequence2("P2"); }
                    else if (currentLevel == "P2") { loadSequence2("P3"); }
                    else if (currentLevel == "P3") { loadSequence2("P4"); }
                    else if (currentLevel == "P4") { loadStage1Eval(); } 
            }
            else if (evalActive) { loadLevelSelect(); }
        }
    }

    public void loadNextLevel(float delayInSeconds)
    {
        StartCoroutine(waitAndLoadNextLevel(delayInSeconds));
    }

    public IEnumerator waitAndLoadNextLevel(float delay)
    {
        ////LogData.instance.AddToLogs("Loading Next Level after " + currentLevel);

        yield return new WaitForSeconds(delay);
        /*
        if (currentLevel == "Intro") { loadTutorial1(); }
        else if (currentLevel == "Tutorial1") { loadTutorial2(); }
        else if (currentLevel == "Tutorial2") { loadTutorial3(); }
        else if (currentLevel == "Tutorial3") { loadPractice1(); }
        else if (currentLevel == "Practice1") { loadPractice2(); }
        else if (currentLevel == "Practice2") { loadPractice3(); }
        else if (currentLevel == "Practice3") { loadPractice4(); }
        else if (currentLevel == "Practice4") { loadPractice5(); }
        else if (currentLevel == "Practice5") { loadPractice6(); }
        else if (currentLevel == "Practice6") { loadPractice7(); }
        else if (currentLevel == "Practice7") { loadEval1(); }
        */

        if (currentStage == 1)
        {
            if (currentSequence == 1)
            {
                if (currentLevel == "T1") { loadSequence1("T2"); }
                else if (currentLevel == "P1") { loadSequence1("P2"); }
                else if (currentLevel == "P2") { loadSequence1("P3"); }
                else if (currentLevel == "P3") { loadSequence1("P4"); }
                else if (currentLevel == "P4") { loadSequence2("T1"); }
            }
            else if (currentSequence == 2)
            {
                if (currentLevel == "T1") { loadSequence2("T2"); }
                else if (currentLevel == "P1") { loadSequence2("P2"); }
                else if (currentLevel == "P2") { loadSequence2("P3"); }
                else if (currentLevel == "P3") { loadSequence2("P4"); }
                else if (currentLevel == "P4") { loadStage1Eval(); }
            }
            else if (evalActive) { loadLevelSelect(); }
        }
    }
    void compareScene(string neededScene)
    {
        Debug.Log("comparing scene");
        activeScene = SceneManager.GetActiveScene();
        string sceneName = activeScene.name;
        if (sceneName != neededScene)
        {
            SceneManager.LoadScene(neededScene);
        }
    }

    //private void Start(){loadTrial1();}

    //spawnerScript.spawnObject spawns a random prefab from the list, needs a string of: "Service", "Mad" or "Beskidt" to know which list to spawn from.
    //spawnerScript.spawnThisObject spawns a specific prefab, needs a string of: "b" for beskidt tallerken, "s" for smoer or "r" for ren tallerken, to know which prefab to spawn.
    public void loadIntro()
    {
        currentLevel = "Intro";
        compareScene("Intro");
        annihilation();
        Debug.Log(currentLevel + " is being loaded");
    }
    public void loadTrial1()
    {
        currentLevel = "Trial1";
        compareScene("TrialScene");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        //spawnerScript.spawnObject("Service");
        //spawnerScript.spawnObject("Mad");
        //spawnerScript.spawnObject("Beskidt");
        gameController.rightThreshold = 3;
        Debug.Log(currentLevel + " is being loaded");
    }
    //Stage 1: Plates (Seq1 -> seq2 -> eval)
        //Seq1=
    public void loadCustomSeq1(int cleanTallerken, int dirtyTallerken, bool boolskab, bool boolopvask, bool boolkoele, int rightAnswers) 
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
        gameController.rightThreshold = rightAnswers;
    }
    public void loadSequence1(string levelName)
    {
        currentStage = 1;
        currentSequence = 1;
        compareScene("Tutorial_Practice");
        annihilation();

        if(levelName == "T1") 
        {
            currentLevel = levelName;
            Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
            spawnerScript.spawnThisObject("rT");
            gameController.rightThreshold = 1;
        }
        else if(levelName == "T2")
        {
            currentLevel = levelName;
            Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
            spawnerScript.spawnThisObject("bT");
            gameController.rightThreshold = 1;
        }
        else if(levelName == "P1")
        {
            currentLevel = levelName;

            Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
            Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);

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
    }
    public void loadSequence2(string levelName)
    {
        currentStage = 1;
        currentSequence = 2;
        compareScene("Tutorial_Practice");
        annihilation();


        if (levelName == "T1")
        {
            currentLevel = levelName;
            Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
            spawnerScript.spawnThisObject("rG");
            gameController.rightThreshold = 1;
        }
        else if (levelName == "T2")
        {
            currentLevel = levelName;
            Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
            spawnerScript.spawnThisObject("bG");
            gameController.rightThreshold = 1;
        }
        else if (levelName == "P1")
        {
            currentLevel = levelName;

            Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
            Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);

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
        else { Debug.Log(levelName + " is invalid"); }

    }
    public void loadStage1Eval()
    {
        currentStage = 1;
        currentSequence = 3;
        evalActive = true;
        compareScene("Evaluation");

        //annihilation();
        //Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        //Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);

        for (int i = 0; i < 2; i++)
        {
            spawnerScript.spawnThisObject("rT");
            spawnerScript.spawnThisObject("bT");
            spawnerScript.spawnThisObject("rG");
            spawnerScript.spawnThisObject("bG");
        }
    }

    public void loadLevelSelect() 
    {
        currentLevel = "LevelSelect";
        compareScene("LevelSelect");
        Debug.Log(currentLevel + " is being loaded");
        ////LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
    public void loadVR()
    {
        currentLevel = "VR";
        compareScene("VRTutorial");
        gameController.resetScore();
        Debug.Log(currentLevel + " is being loaded");
        ////LogData.instance.AddToLogs(currentLevel + " is being loaded");
        gameController.rightThreshold = 2;
    }

    //old levels ______________________________________________________________________________________________________________________________________________________________________________________
    public void loadTutorial1()
    {
        compareScene("Tutorial_Practice");
        currentLevel = "Tutorial1";
        annihilation();
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        spawnerScript.spawnThisObject("bT");
        //need to spawn dirty plate, don't have that yet
        gameController.rightThreshold = 1;
        Debug.Log(currentLevel + " is being loaded");
        //////LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
    public void loadTutorial2()
    {
        currentLevel = "Tutorial2";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        spawnerScript.spawnThisObject("s");
        gameController.rightThreshold = 1;
        Debug.Log(currentLevel + " is being loaded");
        //////LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
    public void loadTutorial3()
    {
        currentLevel = "Tutorial3";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("rT");
        gameController.rightThreshold = 1;
        Debug.Log(currentLevel + " is being loaded");
        //////LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
    public void loadPractice1()
    {
        currentLevel = "Practice1";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("bT");
        gameController.rightThreshold = 1;
        Debug.Log(currentLevel + " is being loaded");
        //////LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
    public void loadPractice2()
    {
        currentLevel = "Practice2";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("s");
        gameController.rightThreshold = 1;
        Debug.Log(currentLevel + " is being loaded");
        //////LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
    public void loadPractice3()
    {
        currentLevel = "Practice3";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("rT");
        gameController.rightThreshold = 1;
        Debug.Log(currentLevel + " is being loaded");
        //////LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
    public void loadPractice4()
    {
        currentLevel = "Practice4";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("bT");
        spawnerScript.spawnThisObject("s");
        gameController.rightThreshold = 2;
        Debug.Log(currentLevel + " is being loaded");
        //////LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
    public void loadPractice5()
    {
        currentLevel = "Practice5";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("s");
        spawnerScript.spawnThisObject("rT");
        gameController.rightThreshold = 2;
        Debug.Log(currentLevel + " is being loaded");
        //////LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
    public void loadPractice6()
    {
        currentLevel = "Practice6";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("bT");
        spawnerScript.spawnThisObject("rT");
        gameController.rightThreshold = 2;
        Debug.Log(currentLevel + " is being loaded");
        //////LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
    public void loadPractice7()
    {
        currentLevel = "Practice7";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("bT");
        spawnerScript.spawnThisObject("rT");
        spawnerScript.spawnThisObject("s");
        gameController.rightThreshold = 3;
        Debug.Log(currentLevel + " is being loaded");
        //////LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
    public void loadEval1()
    {
        currentLevel = "Eval1";

        SceneManager.LoadScene("Evaluation");
        gameController.resetScore();

        //compareScene("Evaluation");

        //annihilation();
        //Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        //Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        //Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        //spawnerScript.spawnThisObject("b");
        //spawnerScript.spawnThisObject("b");
        //spawnerScript.spawnThisObject("r");
        //spawnerScript.spawnThisObject("r");
        //spawnerScript.spawnThisObject("s");
        //spawnerScript.spawnThisObject("s");

        Debug.Log(currentLevel + " is being loaded");
        ////LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
}
