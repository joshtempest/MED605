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

        // DEBUG: Check how many points are registered at the start of the game
        Debug.Log($"[Receiver: {gameObject.name}] Initialized with {displayPoints.Count} display points.");
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
        Debug.Log($"Trigger entered by {other.gameObject.name}");

        if (isServiceReceiver)
        {
            if (other.gameObject.tag == "Service")
                HandleCorrectItem(other.gameObject);
            else if (other.gameObject.tag == "Beskidt")
                HandleWrongItem(other.gameObject, "b");
            else if (other.gameObject.tag == "Mad")
                HandleWrongItem(other.gameObject, "s");
        }
        else if (isBeskidtReceiver)
        {
            if (other.gameObject.tag == "Beskidt")
                HandleCorrectItem(other.gameObject);
            else if (other.gameObject.tag == "Service")
                HandleWrongItem(other.gameObject, "r");
            else if (other.gameObject.tag == "Mad")
                HandleWrongItem(other.gameObject, "s");
        }
        else if (isMadReceiver)
        {
            if (other.gameObject.tag == "Mad")
                HandleCorrectItem(other.gameObject);
            else if (other.gameObject.tag == "Service")
                HandleWrongItem(other.gameObject, "r");
            else if (other.gameObject.tag == "Beskidt")
                HandleWrongItem(other.gameObject, "b");
        }
    }

    private void HandleCorrectItem(GameObject item)
    {
        Debug.Log($"=== HandleCorrectItem triggered for {item.name} ===");

        gameController.AddLog(item.name, this.gameObject.name, true);
        gameController.increaseScore(1);

        // Position the item
        if (displayPoints != null && displayPoints.Count > 0)
        {
            int index = Mathf.Min(currentDisplayIndex, displayPoints.Count - 1);
            Transform targetPoint = displayPoints[index];

            Debug.Log($"Attempting to snap '{item.name}' to point index {index} ('{targetPoint.name}') at position {targetPoint.position}.");

            item.transform.position = targetPoint.position;
            item.transform.rotation = targetPoint.rotation;
            item.transform.SetParent(targetPoint);

            Debug.Log($"Success! '{item.name}' is now at {item.transform.position} and parented to '{item.transform.parent?.name}'.");

            currentDisplayIndex++;
        }
        else
        {
            Debug.LogWarning($"No Display Points assigned for '{this.gameObject.name}'! Falling back to snapping item directly to receiver center.");
            item.transform.position = transform.position;
            item.transform.SetParent(transform);
        }

        // Disable physics
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero; // Stop any existing movement
            rb.angularVelocity = Vector3.zero; // Stop any existing spinning
            Debug.Log($"Disabled Rigidbody on '{item.name}'.");
        }
        else
        {
            Debug.LogWarning($"Could not find a Rigidbody on '{item.name}' to disable! Is it located on a child object?");
        }

        Collider col = item.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
            Debug.Log($"Disabled Collider on '{item.name}'.");
        }
        else
        {
            Debug.LogWarning($"Could not find a Collider on '{item.name}' to disable! Is it located on a child object?");
        }

        PlayParticles(true);
        AudioManager.Instance.PlaySFX("Victory");

        Debug.Log($"=== Finished HandleCorrectItem for {item.name} ===");
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