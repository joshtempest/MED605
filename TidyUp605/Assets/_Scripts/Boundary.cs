using Unity.VisualScripting;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    string[] objectTags = { "Fork", "Knife", "Spoon"};

    

    public void OnCollisionEnter(Collision other)
    {
        Vector3 spawnPos = new Vector3(0f, 1.1f, 0f);
        // If collided object matches one of the tags, respawn a copy at the spawn position
        for (int i = 0; i < objectTags.Length; i++)
        {
            string currentTag = objectTags[i];

            if (other.gameObject.tag == currentTag)
            {
                // instantiate a copy at the spawn position then destroy the original
                Instantiate(other.gameObject, spawnPos, Quaternion.identity);
                Destroy(other.gameObject);
                break;
            }
        }
    }
}
