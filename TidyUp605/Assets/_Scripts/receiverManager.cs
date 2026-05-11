using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class receiverManager : MonoBehaviour
{
    private GameObject GameManager;
    private objectSpawner spawnerScript;
    private GameController gameController;
    private Identifier identityScript;

    [Header("Receiver Types")]
    [SerializeField] private bool isServiceReceiver;
    [SerializeField] private bool isBeskidtReceiver;
    [SerializeField] private bool isMadReceiver;
    private int receiverTypeCount;

    [Header("Particle Systems")]
    [SerializeField] private GameObject go_part_tick;
    [SerializeField] private GameObject go_part_sparkles;
    [SerializeField] private GameObject go_part_x;

    private ParticleSystem part_tick;
    private ParticleSystem part_sparkles;
    private ParticleSystem part_x;

    bool particlesOnline = false;

    [Header("Placement Settings (Assign Hidden Objects Here)")]
    [Tooltip("Assign inactive plates here")]
    [SerializeField] private List<GameObject> platesToReveal = new List<GameObject>();
    [Tooltip("Assign inactive forks here")]
    [SerializeField] private List<GameObject> forksToReveal = new List<GameObject>();
    [Tooltip("Assign inactive food here")]
    [SerializeField] private List<GameObject> foodToReveal = new List<GameObject>();


    private void Awake()
    {
        spawnerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<objectSpawner>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void Start()
    {
        receiverTypeCount = (isServiceReceiver ? 1 : 0) + (isBeskidtReceiver ? 1 : 0) + (isMadReceiver ? 1 : 0);
        if (receiverTypeCount > 1)
        {
            Debug.LogWarning((this.gameObject.name) + " has " + receiverTypeCount + " types assigned. Please assign only one type to the receiver.");
            this.gameObject.GetComponent<receiverManager>().enabled = false;
        }
        if (!isServiceReceiver && !isBeskidtReceiver && !isMadReceiver)
        {
            Debug.LogWarning((this.gameObject.name) + " has no type assigned. Please assign a type to the receiver.");
            this.gameObject.GetComponent<receiverManager>().enabled = false;
        }

        particlesOnline = InitialiseParticles();

        // Reveal any items that were already scored
        if (isServiceReceiver)
        {
            revelio(gameController.plateAnswers, platesToReveal);
            revelio(gameController.cleanFAnswers, forksToReveal);
        }
        else if (isBeskidtReceiver)
        {
            revelio(gameController.dirtyPAnswers, platesToReveal);
            revelio(gameController.dirtyFAnswers, forksToReveal);
        }
        else if (isMadReceiver)
        {
            revelio(gameController.smoerAnswers, foodToReveal);
        }
    }

    public bool InitialiseParticles()
    {
        part_tick = go_part_tick.GetComponent<ParticleSystem>();
        part_sparkles = go_part_sparkles.GetComponent<ParticleSystem>();
        part_x = go_part_x.GetComponent<ParticleSystem>();

        if (!part_tick || !part_sparkles || !part_x)
        {
            Debug.LogWarning($"Particle system not detected: tick is {part_tick} - sparkles is {part_sparkles} - x is {part_x}");
            return false;
        }
        else
        {
            return true;
        }
    }


    //  This triggers when a physical object is dropped into the receiver's trigger zone.
    // It asks two questions: "Who are you?" (Identity) and "Do you belong here?" (Tags).
    private void OnTriggerEnter(Collider other)
    {
        // Get the identity script of the incoming object to determine what specific item it is
        identityScript = other.gameObject.GetComponent<Identifier>();

        if (isServiceReceiver)
        {
            if (other.gameObject.tag == "Service")
            {
                string objID = identityScript.IdentifyObject();

                // If it's a Clean Plate
                if (objID == "rT")
                {
                    gameController.plateAnswers++;
                    revelio(gameController.plateAnswers, platesToReveal);
                }
                // If it's a Clean Fork
                else if (objID == "rG")
                {
                    gameController.cleanFAnswers++;
                    revelio(gameController.cleanFAnswers, forksToReveal);
                }

                HandleCorrectItem(other.gameObject);
            }
            else
                HandleWrongItem(other.gameObject, identityScript.IdentifyObject());
        }
        else if (isBeskidtReceiver)
        {
            if (other.gameObject.tag == "Beskidt")
            {
                string objID = identityScript.IdentifyObject();

                // If it's a Dirty Plate
                if (objID == "bT")
                {
                    gameController.dirtyPAnswers++;
                    revelio(gameController.dirtyPAnswers, platesToReveal);
                }
                // If it's a Dirty Fork
                else if (objID == "bG")
                {
                    gameController.dirtyFAnswers++;
                    revelio(gameController.dirtyFAnswers, forksToReveal);
                }

                HandleCorrectItem(other.gameObject);
            }
            else
                HandleWrongItem(other.gameObject, identityScript.IdentifyObject());
        }
        else if (isMadReceiver)
        {
            if (other.gameObject.tag == "Mad")
            {
                gameController.smoerAnswers++;
                revelio(gameController.smoerAnswers, foodToReveal);

                HandleCorrectItem(other.gameObject);
            }
            else
                HandleWrongItem(other.gameObject, identityScript.IdentifyObject());
        }
    }


    // Adds score, destroys the physics object, and plays victory effects.

    private void HandleCorrectItem(GameObject item)
    {
        Debug.Log($"=== HandleCorrectItem triggered for {item.name} ===");

        gameController.AddLog(item.name, this.gameObject.name, true);
        gameController.increaseScore(1);

        Destroy(item);
        Debug.Log($"Destroyed incoming item: {item.name}");

        PlayParticles(true);
        AudioManager.Instance.PlaySFX("Victory");

        Debug.Log($"=== Finished HandleCorrectItem ===");
    }

    // this looks at the score and turns on a pre placed hidden object
    private void revelio(int revealCount, List<GameObject> listToReveal)
    {
        // Safety check to prevent errors if a list is empty
        if (listToReveal == null || listToReveal.Count == 0) return;

        // Loop through the list of hidden objects and turn on the correct amount
        for (int i = 0; i < listToReveal.Count; i++)
        {
            if (i < revealCount)
            {
                listToReveal[i].SetActive(true);
            }
        }
    }

    // Subtracts score, destroys the wrong item, and tells the spawner to give them a new one.
    private void HandleWrongItem(GameObject item, string respawnCode)
    {
        gameController.AddLog(item.name, this.gameObject.name, false);
        gameController.decreaseScore(1);

        // Destroy the incorrect item
        Destroy(item);

        // Build a replacement
        spawnerScript.spawnThisObject(respawnCode);

        PlayParticles(false);
        AudioManager.Instance.PlaySFX("Wrong");
    }

    public void PlayParticles(bool isSuccessful)
    {
        if (!particlesOnline) return;

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