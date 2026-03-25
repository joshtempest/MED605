using UnityEngine;

public class objectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private GameObject spawnPlatform;

    void Start()
    {
        if (spawnPlatform == null)
        {
            Debug.LogError("Spawn platform is not assigned in the inspector.");
            spawnPlatform = this.gameObject; // Default to the current game object if not assigned
        }
        Vector3 spawnBuffer = new Vector3(0f, 0.2f, 0f);
        Vector3 spawnPosition = spawnPlatform.gameObject.transform.position + spawnBuffer;
        spawnObject(prefabToSpawn, spawnPosition);
    }
    public void spawnObject(GameObject objectToSpawn, Vector3 positionToSpawn)
    {
        Instantiate(objectToSpawn, positionToSpawn, Quaternion.identity);
    }
}
