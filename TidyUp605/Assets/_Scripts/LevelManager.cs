using UnityEngine;

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

    //to (hopefully) avoid the objects spawning in the floor
    Vector3 koeleSpawnbuffer = new Vector3(0, 1.62f, 0);
    Vector3 skabSpawnbuffer = new Vector3(0, 0.1f, 0);
    Vector3 opvaskeSpawnbuffer = new Vector3(0, 0.1f, 0);

    Vector3 koelePlatformPos;
    Vector3 skabPlatformPos;
    Vector3 opvaskemaskinePlatformPos;

    string currentLevel;

    private void Awake()
    {
        spawnerScript = this.gameObject.GetComponent<objectSpawner>();

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
    }

    //private void Start(){loadTrial1();}

    void reloadLevel()
    {
        if (currentLevel == "Trial1")
        {
            loadTrial1();
        }
        else if (currentLevel == "Tutorial1")
        {
            loadTutorial1();
        }
    }

    void loadTrial1()
    {
        annihilation();
        currentLevel = "Trial1";
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
    }

    void loadTutorial1()
    {
        annihilation();
        currentLevel = "Tutorial1";
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        //spawnerScript.spawnObject("Beskidt");
        //need to spawn dirty plate, don't have that yet
    }

    void loadTutorial2()
    {
        annihilation();
        currentLevel = "Tutorial2";
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        spawnerScript.spawnObject("Mad");
    }

    void loadTutorial3()
    {
        annihilation();
        currentLevel = "Tutorial3";
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnObject("Service");
    }

    void loadPractice1() 
    {
        annihilation();
        currentLevel = "Practice1";
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        //spawnerScript.spawnObject("Beskidt");
    }
    void loadPractice2()
    {
        annihilation();
        currentLevel = "Practice2";
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnObject("Mad");
    }

    void loadPractice3()
    {
        annihilation();
        currentLevel = "Practice3";
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnObject("Service");
    }
}
