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
                gameController.AddLog(collision.gameObject.name, this.gameObject.name, true);
                Destroy(collision.gameObject);
                gameController.increaseScore(1);
                PlayParticles(true);
                AudioManager.Instance.PlaySFX("Victory");
            }
            else if (collision.gameObject.tag == "Beskidt")
            {
                gameController.AddLog(collision.gameObject.name, this.gameObject.name, false);
                Destroy(collision.gameObject);
                gameController.decreaseScore(1);
                spawnerScript.spawnThisObject("b");
                PlayParticles(false);
                AudioManager.Instance.PlaySFX("Wrong");
            }
            else if (collision.gameObject.tag == "Mad")
            {
                gameController.AddLog(collision.gameObject.name, this.gameObject.name, false);
                Destroy(collision.gameObject);
                gameController.decreaseScore(1);
                spawnerScript.spawnThisObject("s");
                PlayParticles(false);
                AudioManager.Instance.PlaySFX("Wrong");
            }
        }
        if (isBeskidtReceiver)
        {
            Debug.Log($"Item collided with a beskidt receiver.");
            if (collision.gameObject.tag == "Beskidt")
            {
                gameController.AddLog(collision.gameObject.name, this.gameObject.name, true);
                Destroy(collision.gameObject);
                gameController.increaseScore(1);
                PlayParticles(true);
                AudioManager.Instance.PlaySFX("Victory");
            }
            else if (collision.gameObject.tag == "Service")
            {
                gameController.AddLog(collision.gameObject.name, this.gameObject.name, false);
                Destroy(collision.gameObject);
                gameController.decreaseScore(1);
                spawnerScript.spawnThisObject("r");
                PlayParticles(false);
                AudioManager.Instance.PlaySFX("Wrong");
            }
            else if (collision.gameObject.tag == "Mad")
            {
                gameController.AddLog(collision.gameObject.name, this.gameObject.name, false);
                Destroy(collision.gameObject);
                gameController.decreaseScore(1);
                spawnerScript.spawnThisObject("s");
                PlayParticles(false);
                AudioManager.Instance.PlaySFX("Wrong");
            }
        }
        if (isMadReceiver)
        {
            Debug.Log($"Item collided with a mad receiver.");
            if (collision.gameObject.tag == "Mad")
            {
                gameController.AddLog(collision.gameObject.name, this.gameObject.name, true);
                Destroy(collision.gameObject);
                gameController.increaseScore(1);
                PlayParticles(true);
                AudioManager.Instance.PlaySFX("Victory");
            }
            else if (collision.gameObject.tag == "Service")
            {
                gameController.AddLog(collision.gameObject.name, this.gameObject.name, false);
                Destroy(collision.gameObject);
                gameController.decreaseScore(1);
                spawnerScript.spawnThisObject("r");
                PlayParticles(false);
                AudioManager.Instance.PlaySFX("Wrong");
            }
            else if (collision.gameObject.tag == "Beskidt")
            {
                gameController.AddLog(collision.gameObject.name, this.gameObject.name, false);
                Destroy(collision.gameObject);
                gameController.decreaseScore(1);
                spawnerScript.spawnThisObject("b");
                PlayParticles(false);
                AudioManager.Instance.PlaySFX("Wrong");
            }
        }
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