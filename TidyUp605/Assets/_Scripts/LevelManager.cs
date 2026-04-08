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

    public void reloadLevel()
    {
        if (currentLevel == "Trial1"){loadTrial1();}
        else if (currentLevel == "Tutorial1"){loadTutorial1();}
        else if(currentLevel == "Tutorial2"){loadTutorial2();}
        else if(currentLevel == "Tutorial3"){loadTutorial3();}
        else if(currentLevel == "Practice1"){loadPractice1();}
        else if(currentLevel == "Practice2"){loadPractice2();}
        else if(currentLevel == "Practice3"){loadPractice3();}
        else if(currentLevel == "Practice4"){loadPractice4();}
        else if(currentLevel == "Practice5"){loadPractice5();}
        else if(currentLevel == "Practice6"){loadPractice6(); }
    }


    //spawnerScript.spawnObject spawns a random prefab from the list, needs a string of: "Service", "Mad" or "Beskidt" to know which list to spawn from.
    //spawnerScript.spawnThisObject spawns a specific prefab, needs a string of: "b" for beskidt tallerken, "s" for smoer or "r" for ren tallerken, to know which prefab to spawn.
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
        spawnerScript.spawnThisObject("b");
        //need to spawn dirty plate, don't have that yet
    }

    void loadTutorial2()
    {
        annihilation();
        currentLevel = "Tutorial2";
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        spawnerScript.spawnThisObject("s");
    }

    void loadTutorial3()
    {
        annihilation();
        currentLevel = "Tutorial3";
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("r");
    }

    void loadPractice1() 
    {
        annihilation();
        currentLevel = "Practice1";
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("b");
    }
    void loadPractice2()
    {
        annihilation();
        currentLevel = "Practice2";
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("s");
    }

    void loadPractice3()
    {
        annihilation();
        currentLevel = "Practice3";
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("r");
    }
    void loadPractice4()
    {
        annihilation();
        currentLevel = "Practice4";
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("b");
        spawnerScript.spawnThisObject("s");
    }
    void loadPractice5()
    {
        annihilation();
        currentLevel = "Practice5";
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("s");
        spawnerScript.spawnThisObject("r");
    }
    void loadPractice6()
    {
        annihilation();
        currentLevel = "Practice6";
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
        Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation);
        spawnerScript.spawnThisObject("b");
        spawnerScript.spawnThisObject("r");
    }
}
