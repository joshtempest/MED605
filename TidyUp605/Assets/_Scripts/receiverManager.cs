using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class receiverManager : MonoBehaviour
{
    private GameObject GameManager;
    private objectSpawner spawnerScript;
    private GameController gameController;

    [SerializeField] private bool isServiceReceiver;
    [SerializeField] private bool isBeskidtReceiver;
    [SerializeField] private bool isMadReceiver;
    private int receiverTypeCount;

    [SerializeField] private GameObject go_part_tick;
    [SerializeField] private GameObject go_part_sparkles;
    [SerializeField] private GameObject go_part_x;

    private ParticleSystem part_tick;
    private ParticleSystem part_sparkles;
    private ParticleSystem part_x;


    bool particlesOnline = false;

   
    [SerializeField] private List<Transform> displayPoints = new List<Transform>();
    private int currentDisplayIndex = 0;


    private void Awake()
    {
        spawnerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<objectSpawner>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void Start()
    {
        //Fancy way to check if more than one type is assigned to the receiver and log a warning if so, also checks if no type is assigned and logs a warning if so
        receiverTypeCount = (isServiceReceiver ? 1 : 0) + (isBeskidtReceiver ? 1 : 0) + (isMadReceiver ? 1 : 0);
        if (receiverTypeCount > 1)
        {
            Debug.LogWarning((this.gameObject) + " has " + receiverTypeCount + " types assigned. Please assign only one type to the receiver.");
            this.gameObject.GetComponent<receiverManager>().enabled = false;
        }
        if (!isServiceReceiver && !isBeskidtReceiver && !isMadReceiver)
        {
            Debug.LogWarning((this.gameObject) + " has no type assigned. Please assign a type to the receiver.");
            this.gameObject.GetComponent<receiverManager>().enabled = false;
        }

        particlesOnline = InitialiseParticles();


    }

    public bool InitialiseParticles()
        { 
        //get access to the particle systems of the receiver
        part_tick = go_part_tick.GetComponent<ParticleSystem>();
        //part_tick3 = this.gameObject.GetComponentInChildren<ParticleSystem>();
        part_sparkles = go_part_sparkles.GetComponent<ParticleSystem>();
        part_x = go_part_x.GetComponent<ParticleSystem>();
       

        //throw a warning if the particle systems aren't found
        if (!part_tick || !part_sparkles || !part_x)
        {
            Debug.LogWarning($"Particle system not detected: tick is {part_tick} - sparkles is {part_sparkles} - x is {part_x}");
            return false;
        }
        else 
        {
            //Debug.Log($"Particle systems assigned: tick is {part_tick} - sparkles is {part_sparkles} - x is {part_x}");

            return true;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision detected with {collision.gameObject.name}");
        
        if (isServiceReceiver) 
        {
            Debug.Log($"Item collided with a service receiver.");

            if (collision.gameObject.tag == "Service")
            {
                //Add interaction to logs
                gameController.AddLog(collision.gameObject.name, this.gameObject.name, true);
                
                //Add score for positive interaction
                Destroy(collision.gameObject);
                gameController.increaseScore(1);

                //play success particles
                PlayParticles(true);

                //play positive sound effect
                AudioManager.Instance.PlaySFX("Victory");
            }
            else if (collision.gameObject.tag == "Beskidt")
            {
                //Add interaction to logs
                gameController.AddLog(collision.gameObject.name, this.gameObject.name, false);

                Destroy(collision.gameObject);
                gameController.decreaseScore(1);
                //Subtract score for negative interaction

                spawnerScript.spawnThisObject("b");

                //play error particle
                PlayParticles(false);

                //play negative sound effect
                AudioManager.Instance.PlaySFX("Wrong");

            }
            else if (collision.gameObject.tag == "Mad")
            {
                //Add interaction to logs
                gameController.AddLog(collision.gameObject.name, this.gameObject.name, false);

                Destroy(collision.gameObject);
                gameController.decreaseScore(1);
                //Subtract score for negative interaction

                spawnerScript.spawnThisObject("s");

                //play error particle
                PlayParticles(false);

                //play negative sound effect
                AudioManager.Instance.PlaySFX("Wrong");
            }
        }
        if (isBeskidtReceiver) 
        {
            Debug.Log($"Item collided with a beskidt receiver.");

            if (collision.gameObject.tag == "Beskidt")
            {
                //Add interaction to logs
                gameController.AddLog(collision.gameObject.name, this.gameObject.name, true);

                Destroy(collision.gameObject);
                gameController.increaseScore(1);
                //Add score for positive interaction

                //play success particles
                PlayParticles(true);

                //play positive sound effect
                AudioManager.Instance.PlaySFX("Victory");
            }
            else if (collision.gameObject.tag == "Service")
            {
                //Add interaction to logs
                gameController.AddLog(collision.gameObject.name, this.gameObject.name, false);

                Destroy(collision.gameObject);
                gameController.decreaseScore(1);
                //Subtract score for negative interaction

                spawnerScript.spawnThisObject("r");

                //play error particle
                PlayParticles(false);

                //play negative sound effect
                AudioManager.Instance.PlaySFX("Wrong");

            }
            else if (collision.gameObject.tag == "Mad")
            {
                //Add interaction to logs
                gameController.AddLog(collision.gameObject.name, this.gameObject.name, false);

                Destroy(collision.gameObject);
                gameController.decreaseScore(1);
                //Subtract score for negative interaction

                spawnerScript.spawnThisObject("s");

                //play error particle
                PlayParticles(false);

                //play negative sound effect
                AudioManager.Instance.PlaySFX("Wrong");
            }
        }
        if (isMadReceiver)
        {
            Debug.Log($"Item collided with a mad receiver.");

            if (collision.gameObject.tag == "Mad")
                HandleCorrectItem(collision.gameObject);
            else if (collision.gameObject.tag == "Service")
                HandleWrongItem(collision.gameObject, "r");
            else if (collision.gameObject.tag == "Beskidt")
                HandleWrongItem(collision.gameObject, "b");
        }
    }
    private void HandleCorrectItem(GameObject item)
    {
        // Add interaction to logs & increase score
        gameController.AddLog(item.name, this.gameObject.name, true);
        gameController.increaseScore(1);

        // Position the item instead of destroying it
        if (displayPoints != null && displayPoints.Count > 0)
        {
            // Get the next placement point. If we run out, stack on the last one.
            int index = Mathf.Min(currentDisplayIndex, displayPoints.Count - 1);
            Transform targetPoint = displayPoints[index];

            item.transform.position = targetPoint.position;
            item.transform.rotation = targetPoint.rotation;
            item.transform.SetParent(targetPoint);

            currentDisplayIndex++;
        }
        else
        {
            // Fallback: place at receiver's center if no points are assigned in inspector
            item.transform.position = transform.position;
            item.transform.SetParent(transform);
        }

        // Disable physics to prevent further collisions and falling
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Collider col = item.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        // Play success particles & sound
        PlayParticles(true);
        AudioManager.Instance.PlaySFX("Victory");
    }
    private void HandleWrongItem(GameObject item, string respawnCode)
    {
        // Add interaction to logs & decrease score
        gameController.AddLog(item.name, this.gameObject.name, false);
        gameController.decreaseScore(1);

        // Destroy the incorrect object and respawn
        Destroy(item);
        spawnerScript.spawnThisObject(respawnCode);

        // Play error particle & negative sound effect
        PlayParticles(false);
        AudioManager.Instance.PlaySFX("Wrong");
    }

    public void PlayParticles(bool isSuccessful)
    {
        if (!particlesOnline)
            return;

        if (isSuccessful)
        {
            part_tick.Play();
            part_sparkles.Play();
        }
        else
        {
            part_x.Play();
        }
    }
}
