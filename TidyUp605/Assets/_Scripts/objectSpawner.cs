using UnityEngine;
using UnityEngine.Rendering;

public class objectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject forkPrefabToSpawn;
    [SerializeField] private GameObject smoerPrefabToSpawn;
    //[SerializeField] private GameObject prefabToSpawn;

    [SerializeField] private GameObject forkSpawnPlatform;
    [SerializeField] private GameObject smoerSpawnPlatform;
    Vector3 spawnBuffer = new Vector3(0f, 0.2f, 0f);

    void Awake()
    {
        Vector3 forkSpawnPlat = forkSpawnPlatform.transform.position;
        Vector3 smoerSpawnPlat = smoerSpawnPlatform.transform.position;
    }

    void Start()
    {
        spawnObject("Fork");
        spawnObject("Smoer");
    }
    public void spawnObject(string tag)
    {
        if (tag =="Fork")
        {
            Vector3 positionToSpawn = forkSpawnPlatform.transform.position + spawnBuffer;
            Instantiate(forkPrefabToSpawn, positionToSpawn, Quaternion.identity);
        }
        if (tag == "Smoer")
        {
            Vector3 positionToSpawn = smoerSpawnPlatform.transform.position + spawnBuffer;
            Instantiate(smoerPrefabToSpawn, positionToSpawn, Quaternion.identity);
        }
    }
}
