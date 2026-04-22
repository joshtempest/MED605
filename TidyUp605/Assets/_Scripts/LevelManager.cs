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
    public string currentLevel;
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
        LogData.instance.AddToLogs("Annihalating...");
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
        LogData.instance.AddToLogs("Reloading level: " + currentLevel);
        if (currentLevel == "Trial1"){loadTrial1();}
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
        LogData.instance.AddToLogs("Reloading level: " + currentLevel);
        if (currentLevel == "Trial1") { loadTrial1(); }
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
        else if (currentLevel == "Intro") { loadIntro(); }
        else if (currentLevel == "LevelSelect") { loadLevelSelect(); }
        else if (currentLevel == "VR") { loadVR(); }
    }



    public void loadNextLevel()
    {
        LogData.instance.AddToLogs("Loading Next Level after " + currentLevel);
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
    }

    public void loadNextLevel(float delayInSeconds)
    {
        StartCoroutine(waitAndLoadNextLevel(delayInSeconds));
    }

    public IEnumerator waitAndLoadNextLevel(float delay)
    {
        LogData.instance.AddToLogs("Loading Next Level after " + currentLevel);

        yield return new WaitForSeconds(delay);

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
        spawnerScript.spawnObject("Service");
        spawnerScript.spawnObject("Mad");
        spawnerScript.spawnObject("Beskidt");
        gameController.rightThreshold = 3;
        Debug.Log(currentLevel + " is being loaded");
    }

    public void loadTutorial1()
    {
        currentLevel = "Tutorial1";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        spawnerScript.spawnThisObject("b");
        //need to spawn dirty plate, don't have that yet
        gameController.rightThreshold = 1;
        Debug.Log(currentLevel + " is being loaded");
        LogData.instance.AddToLogs(currentLevel + " is being loaded");
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
        LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }

    public void loadTutorial3()
    {
        currentLevel = "Tutorial3";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("r");
        gameController.rightThreshold = 1;
        Debug.Log(currentLevel + " is being loaded");
        LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
    public void loadPractice1() 
    {
        currentLevel = "Practice1";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("b");
        gameController.rightThreshold = 1;
        Debug.Log(currentLevel + " is being loaded");
        LogData.instance.AddToLogs(currentLevel + " is being loaded");
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
        LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
    public void loadPractice3()
    {
        currentLevel = "Practice3";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("r");
        gameController.rightThreshold = 1;
        Debug.Log(currentLevel + " is being loaded");
        LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
    public void loadPractice4()
    {
        currentLevel = "Practice4";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("b");
        spawnerScript.spawnThisObject("s");
        gameController.rightThreshold = 2;
        Debug.Log(currentLevel + " is being loaded");
        LogData.instance.AddToLogs(currentLevel + " is being loaded");
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
        spawnerScript.spawnThisObject("r");
        gameController.rightThreshold = 2;
        Debug.Log(currentLevel + " is being loaded");
        LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
    public void loadPractice6()
    {
        currentLevel = "Practice6";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("b");
        spawnerScript.spawnThisObject("r");
        gameController.rightThreshold = 2;
        Debug.Log(currentLevel + " is being loaded");
        LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
    public void loadPractice7()
    {
        currentLevel = "Practice7";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("b");
        spawnerScript.spawnThisObject("r");
        spawnerScript.spawnThisObject("s");
        gameController.rightThreshold = 3;
        Debug.Log(currentLevel + " is being loaded");
        LogData.instance.AddToLogs(currentLevel + " is being loaded");
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
        LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }

    public void loadLevelSelect() 
    {
        currentLevel = "LevelSelect";
        compareScene("LevelSelect");
        Debug.Log(currentLevel + " is being loaded");
        LogData.instance.AddToLogs(currentLevel + " is being loaded");
    }
    public void loadVR()
    {
        currentLevel = "VR";
        compareScene("VRTutorial");
        gameController.resetScore();
        Debug.Log(currentLevel + " is being loaded");
        LogData.instance.AddToLogs(currentLevel + " is being loaded");
        gameController.rightThreshold = 2;
    }
}
