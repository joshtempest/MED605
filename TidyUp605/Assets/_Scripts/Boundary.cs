using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    [SerializeField] private GameObject GameManager;
    private objectSpawner spawnerScript;
    string[] objectTags = { "Fork", "Knife", "Spoon", "Smoer"};

    private void Awake()
    {
        if (GameManager != null)
        {
            spawnerScript = GameManager.GetComponent<objectSpawner>();
        }
        else { 
            spawnerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<objectSpawner>();
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        // If collided object matches one of the tags, respawn a copy at the spawn position
        for (int i = 0; i < objectTags.Length; i++)
        {
            string currentTag = objectTags[i];

            if (other.gameObject.tag == currentTag)
            {
                Destroy(other.gameObject);
                spawnerScript.spawnObject(currentTag);
                break;
            }
        }
    }
}
