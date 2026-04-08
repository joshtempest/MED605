using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //Prefab
    [SerializeField] GameObject koeleskab;
    [SerializeField] GameObject skab;
    [SerializeField] GameObject opvaskemaskine;

    //Platforms (to place them the same place everytime)
    [SerializeField] GameObject koelePlatform;
    [SerializeField] GameObject skabPlatform;
    [SerializeField] GameObject opvaskemaskinePlatform;

    //To access the spawning of the sorting objects
    private objectSpawner spawnerScript;
    private GameController gameController;

    //to (hopefully) avoid the objects spawning in the floor
    Vector3 koeleSpawnbuffer = new Vector3(0, 1.62f, 0);
    Vector3 skabSpawnbuffer = new Vector3(0, 0.1f, 0);
    Vector3 opvaskeSpawnbuffer = new Vector3(0, 0.1f, 0);

    Vector3 koelePlatformPos;
    Vector3 skabPlatformPos;
    Vector3 opvaskemaskinePlatformPos;

    string currentLevel;
    Scene activeScene;

    private void Awake()
    {
        spawnerScript = this.gameObject.GetComponent<objectSpawner>();
        gameController = this.gameObject.GetComponent<GameController>();

        koelePlatformPos = koelePlatform.transform.position + koeleSpawnbuffer;
        skabPlatformPos = skabPlatform.transform.position + skabSpawnbuffer;
        opvaskemaskinePlatformPos = opvaskemaskinePlatform.transform.position + opvaskeSpawnbuffer;
    }

    void annihilation() 
    {
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

    //private void Start(){loadTrial1();}

    public void reloadLevel()
    {
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
        else if (currentLevel == "Eval1") {loadEval1(); }
    }

    public void loadNextLevel()
    {
        if (currentLevel == "Trial1") { loadTutorial1(); }
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
        activeScene = SceneManager.GetActiveScene();
        string sceneName = activeScene.name;
        if (sceneName != neededScene)
        {
            SceneManager.LoadScene(neededScene);
        }
    }


    //spawnerScript.spawnObject spawns a random prefab from the list, needs a string of: "Service", "Mad" or "Beskidt" to know which list to spawn from.
    //spawnerScript.spawnThisObject spawns a specific prefab, needs a string of: "b" for beskidt tallerken, "s" for smoer or "r" for ren tallerken, to know which prefab to spawn.
    void loadTrial1()
    {
        currentLevel = "Trial1";
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
    }

    void loadTutorial1()
    {
        currentLevel = "Tutorial1";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        spawnerScript.spawnThisObject("b");
        //need to spawn dirty plate, don't have that yet
    }

    void loadTutorial2()
    {
        currentLevel = "Tutorial2";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        spawnerScript.spawnThisObject("s");
    }

    void loadTutorial3()
    {
        currentLevel = "Tutorial3";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("r");
    }

    void loadPractice1() 
    {
        currentLevel = "Practice1";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("b");
    }
    void loadPractice2()
    {
        currentLevel = "Practice2";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("s");
    }

    void loadPractice3()
    {
        currentLevel = "Practice3";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("r");
    }
    void loadPractice4()
    {
        currentLevel = "Practice4";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("b");
        spawnerScript.spawnThisObject("s");
    }
    void loadPractice5()
    {
        currentLevel = "Practice5";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("s");
        spawnerScript.spawnThisObject("r");
    }
    void loadPractice6()
    {
        currentLevel = "Practice6";
        compareScene("Tutorial_Practice");
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("b");
        spawnerScript.spawnThisObject("r");
    }
    void loadPractice7()
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
    }

    void loadEval1()
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
    }
}
