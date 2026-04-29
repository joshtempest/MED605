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

    [SerializeField] private List<GameObject> objectsToReveal = new List<GameObject>();
    private int currentRevealIndex = 0;


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

        Debug.Log($"[Receiver: {gameObject.name}] Initialized with {objectsToReveal.Count} hidden objects to reveal.");
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

    private void OnTriggerEnter(Collider other)
    {
        identityScript = other.gameObject.GetComponent<Identifier>();

        Debug.Log($"Trigger entered by {other.gameObject.name}");

        if (isServiceReceiver)
        {
            if (other.gameObject.tag == "Service")
                HandleCorrectItem(other.gameObject);
            else
                HandleWrongItem(other.gameObject, identityScript.IdentifyObject());
        }
        else if (isBeskidtReceiver)
        {
            if (other.gameObject.tag == "Beskidt")
                HandleCorrectItem(other.gameObject);
            else
                HandleWrongItem(other.gameObject, identityScript.IdentifyObject());
        }
        else if (isMadReceiver)
        {
            if (other.gameObject.tag == "Mad")
                HandleCorrectItem(other.gameObject);
            else
                HandleWrongItem(other.gameObject, identityScript.IdentifyObject());
        }
    }

    private void HandleCorrectItem(GameObject item)
    {
        Debug.Log($"=== HandleCorrectItem triggered for {item.name} ===");

        gameController.AddLog(item.name, this.gameObject.name, true);
        gameController.increaseScore(1);

        // 1. Destroy the incoming physical item
        Destroy(item);
        Debug.Log($"Destroyed incoming item: {item.name}");

        // 2. Reveal the next hidden object in our list
        if (objectsToReveal != null && currentRevealIndex < objectsToReveal.Count)
        {
            GameObject objectToReveal = objectsToReveal[currentRevealIndex];

            // Turn it on
            objectToReveal.SetActive(true);
            Debug.Log($"Revealed hidden object '{objectToReveal.name}' at index {currentRevealIndex}.");

            // Move to the next index for the next time a correct item is placed
            currentRevealIndex++;
        }
        else
        {
            // If they drop more correct items than you have hidden objects prepared, it logs a warning
            Debug.LogWarning($"Successfully received item, but no more hidden objects left to reveal in the list!");
        }

        PlayParticles(true);
        AudioManager.Instance.PlaySFX("Victory");

        Debug.Log($"=== Finished HandleCorrectItem ===");
    }

    private void HandleWrongItem(GameObject item, string respawnCode)
    {
        gameController.AddLog(item.name, this.gameObject.name, false);
        gameController.decreaseScore(1);

        Destroy(item);
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