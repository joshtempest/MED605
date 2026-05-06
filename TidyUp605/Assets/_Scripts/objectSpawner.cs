using System;
using UnityEngine;
using UnityEngine.Rendering;

public class objectSpawner : MonoBehaviour
{
    /*
    [SerializeField] private GameObject[] servicePrefabsToSpawn;
    [SerializeField] private GameObject[] madPrefabsToSpawn;
    [SerializeField] private GameObject[] beskidtPrefabsToSpawn;
    */
    [Header("Individuelle objekter")]
    [SerializeField] private GameObject smoer;
    [SerializeField] private GameObject beskidtTallerken;
    [SerializeField] private GameObject renTallerken;
    [SerializeField] private GameObject renGaffel;
    [SerializeField] private GameObject beskidtGaffel;
    [SerializeField] private GameObject poelse;

    private GameObject ServiceToSpawn;
    private GameObject MadToSpawn;
    private GameObject BeskidtToSpawn;
    [Header("Spawn Platforms")]
    [SerializeField] private GameObject serviceSpawnPlatform;
    [SerializeField] private GameObject madSpawnPlatform;
    [SerializeField] private GameObject beskidtSpawnPlatform;

    Vector3 spawnBuffer;
    [SerializeField]float spawnbufferDistance = 0.5f;
    float yOffset = 0.2f;

    void Awake()
    {
        Vector3 serviceSpawnPlat = serviceSpawnPlatform.transform.position;
        Vector3 madSpawnPlat = madSpawnPlatform.transform.position;
        Vector3 beskidtSpawnPlat = beskidtSpawnPlatform.transform.position;
    }

    void Start()
    {
        //ServiceToSpawn = servicePrefabsToSpawn[UnityEngine.Random.Range(0, servicePrefabsToSpawn.Length)];
        //MadToSpawn = madPrefabsToSpawn[UnityEngine.Random.Range(0, madPrefabsToSpawn.Length)];
        //BeskidtToSpawn = beskidtPrefabsToSpawn[UnityEngine.Random.Range(0, beskidtPrefabsToSpawn.Length)];
        //spawnObject("Service");
        //spawnObject("Mad");
        //spawnObject("Beskidt");
    }
    /*
    public void spawnObject(string tag)
    {
        if (tag =="Service")
        {
            ServiceToSpawn = servicePrefabsToSpawn[UnityEngine.Random.Range(0, servicePrefabsToSpawn.Length)];

            spawnBuffer = new Vector3 (UnityEngine.Random.Range(-1f, 1f), 0.2f, UnityEngine.Random.Range(-1f, 1f));
            Vector3 positionToSpawn = serviceSpawnPlatform.transform.position + spawnBuffer;

            Instantiate(ServiceToSpawn, positionToSpawn, Quaternion.identity);
            AudioManager.Instance.PlaySFX("Teleport");
        }
        if (tag == "Mad")
        {
            MadToSpawn = madPrefabsToSpawn[UnityEngine.Random.Range(0, madPrefabsToSpawn.Length)];

            spawnBuffer = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0.2f, UnityEngine.Random.Range(-1f, 1f));
            Vector3 positionToSpawn = madSpawnPlatform.transform.position + spawnBuffer;

            Instantiate(MadToSpawn, positionToSpawn, Quaternion.identity);
            AudioManager.Instance.PlaySFX("Teleport");
        }
        if(tag == "Beskidt")
        {
            BeskidtToSpawn = beskidtPrefabsToSpawn[UnityEngine.Random.Range(0, beskidtPrefabsToSpawn.Length)];

            spawnBuffer = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0.2f, UnityEngine.Random.Range(-1f, 1f));
            Vector3 positionToSpawn = beskidtSpawnPlatform.transform.position + spawnBuffer;

            Instantiate(BeskidtToSpawn, positionToSpawn, Quaternion.identity);
            AudioManager.Instance.PlaySFX("Teleport");
        }
    }
    */

    public void spawnThisObject(string type) 
    {
        ///spawn the specific object that was passed in as a parameter, at the correct spawn platform, with a random buffer to prevent stacking
        if (type == "s") //Smoer
        {
            spawnBuffer = new Vector3(UnityEngine.Random.Range(-spawnbufferDistance, spawnbufferDistance), yOffset, UnityEngine.Random.Range(-spawnbufferDistance, spawnbufferDistance));
            Vector3 positionToSpawn = madSpawnPlatform.transform.position + spawnBuffer;

            Instantiate(smoer, positionToSpawn, Quaternion.identity);
        }
        if (type == "bT") //Beskidt Tallerken
        {
            spawnBuffer = new Vector3(UnityEngine.Random.Range(-spawnbufferDistance, spawnbufferDistance), yOffset, UnityEngine.Random.Range(-spawnbufferDistance, spawnbufferDistance));
            Vector3 positionToSpawn = beskidtSpawnPlatform.transform.position + spawnBuffer;

            Instantiate(beskidtTallerken, positionToSpawn, Quaternion.identity);
        }
        if (type == "rT") //Ren Tallerken
        {
            spawnBuffer = new Vector3(UnityEngine.Random.Range(-spawnbufferDistance, spawnbufferDistance), yOffset, UnityEngine.Random.Range(-spawnbufferDistance, spawnbufferDistance));
            Vector3 positionToSpawn = serviceSpawnPlatform.transform.position + spawnBuffer;

            Instantiate(renTallerken, positionToSpawn, Quaternion.identity);
        }
        if (type == "rG") //Ren Gaffel
        {
            spawnBuffer = new Vector3(UnityEngine.Random.Range(-spawnbufferDistance, spawnbufferDistance), yOffset, UnityEngine.Random.Range(-spawnbufferDistance, spawnbufferDistance));
            Vector3 positionToSpawn = beskidtSpawnPlatform.transform.position + spawnBuffer;

            Instantiate(renGaffel, positionToSpawn, Quaternion.identity);
        }
        if (type == "bG") //Beskidt Gaffel
        {
            spawnBuffer = new Vector3(UnityEngine.Random.Range(-spawnbufferDistance, spawnbufferDistance), yOffset, UnityEngine.Random.Range(-spawnbufferDistance, spawnbufferDistance));
            Vector3 positionToSpawn = beskidtSpawnPlatform.transform.position + spawnBuffer;

            Instantiate(beskidtGaffel, positionToSpawn, Quaternion.identity);
        }
        if(type == "p") //Poelse
        {
            spawnBuffer = new Vector3(UnityEngine.Random.Range(-spawnbufferDistance, spawnbufferDistance), yOffset, UnityEngine.Random.Range(-spawnbufferDistance, spawnbufferDistance));
            Vector3 positionToSpawn = madSpawnPlatform.transform.position + spawnBuffer;

            Instantiate(poelse, positionToSpawn, Quaternion.identity);

        }
    }
}
