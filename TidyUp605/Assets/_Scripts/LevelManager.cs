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

    Vector3 koeleSpawnbuffer = new Vector3(0, 1.62f, 0);
    Vector3 skabSpawnbuffer = new Vector3(0, 0.1f, 0);
    Vector3 opvaskeSpawnbuffer = new Vector3(0, 0.1f, 0);

    Vector3 koelePlatformPos;
    Vector3 skabPlatformPos;
    Vector3 opvaskemaskinePlatformPos;

    private void Awake()
    {
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


    void loadTrial1()
    {
        annihilation();
        Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation);
    }

    void loadTutorial1()
    {
        annihilation();
        Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation);
    }
    //private void Start(){loadTrial1();}
}
