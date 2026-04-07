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
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (isServiceReceiver) 
        { 
            if (collision.gameObject.tag == "Service")
            {
                Destroy(collision.gameObject);
                gameController.increaseScore(1);
                //Add score for positive interaction
            }
            else if (collision.gameObject.tag == "Beskidt")
            {
                Destroy(collision.gameObject);
                gameController.decreaseScore(1);
                //Subtract score for negative interaction
            }
            else if (collision.gameObject.tag == "Mad")
            {
                Destroy(collision.gameObject);
                gameController.decreaseScore(1);
                //Subtract score for negative interaction
            }
        }
        if (isBeskidtReceiver) 
        { 
            if(collision.gameObject.tag == "Beskidt")
            {
                Destroy(collision.gameObject);
                gameController.increaseScore(1);
                //Add score for positive interaction
            }
            else if (collision.gameObject.tag == "Service")
            {
                Destroy(collision.gameObject);
                gameController.decreaseScore(1);
                //Subtract score for negative interaction
            }
            else if (collision.gameObject.tag == "Mad")
            {
                Destroy(collision.gameObject);
                gameController.decreaseScore(1);
                //Subtract score for negative interaction
            }
        }
        if (isMadReceiver)
        { 
            if(collision.gameObject.tag == "Mad")
            {
                Destroy(collision.gameObject);
                gameController.increaseScore(1);
                //Add score for positive interaction
            }
            else if (collision.gameObject.tag == "Service")
            {
                Destroy(collision.gameObject);
                gameController.decreaseScore(1);
                //Subtract score for negative interaction
            }
            else if (collision.gameObject.tag == "Beskidt")
            {
                Destroy(collision.gameObject);
                gameController.decreaseScore(1);
                //Subtract score for negative interaction
            }
        }
    }

}
