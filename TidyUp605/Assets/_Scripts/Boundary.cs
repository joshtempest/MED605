using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    private GameObject GameManager;
    private objectSpawner spawnerScript;

    private GameObject collidedObject;
    private Identifier identity;

    string[] objectTags = { "Service", "Beskidt", "Mad"};

    private void Awake()
    {
        spawnerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<objectSpawner>();
    }

    public void OnCollisionEnter(Collision other)
    {
        identity = other.gameObject.GetComponent<Identifier>();
        // If collided object matches one of the tags, respawn a copy at the spawn position
        //for (int i = 0; i < objectTags.Length; i++)
        //{
        //string currentTag = objectTags[i];if (other.gameObject.tag == currentTag){spawnerScript.spawnObject(currentTag);Destroy(other.gameObject);break;}
        if (other.gameObject.tag == "Service") { Destroy(other.gameObject); spawnerScript.spawnThisObject(identity.IdentifyObject()); }
            else if (other.gameObject.tag == "Beskidt") { Destroy(other.gameObject); spawnerScript.spawnThisObject(identity.IdentifyObject()); }
            else if (other.gameObject.tag == "Mad") { Destroy(other.gameObject); spawnerScript.spawnThisObject(identity.IdentifyObject()); }
            else if(other.gameObject.tag == "smoer") { Destroy(other.gameObject); spawnerScript.spawnThisObject(identity.IdentifyObject()); }
            else if(other.gameObject.tag == "plate") { Destroy(other.gameObject); spawnerScript.spawnThisObject(identity.IdentifyObject()); }
            else if(other.gameObject.tag == "beskidtPlate") { Destroy(other.gameObject); spawnerScript.spawnThisObject(identity.IdentifyObject()); }
        //}
    }
}
