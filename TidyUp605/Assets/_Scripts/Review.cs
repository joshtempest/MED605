using UnityEngine;
using UnityEngine.Windows; 

public class TestReview : MonoBehaviour
{
    //assign in inspector

    //overarching Canvas
    public Canvas Review_start;
    public Canvas Review_end;

    //currently a placeholder, add functionality
    public GameObject scoreBar;

    //stars 
    public GameObject starOneYellow;
    public GameObject starOneGrey;
    public GameObject starTwoYellow;
    public GameObject starTwoGrey;
    public GameObject starThreeYellow;
    public GameObject starThreeGrey;

    //log display (=showing what they did right/wrong)
    public GameObject itemRenderer;
    public GameObject receptacleRenderer;
    public GameObject rightWrongIcon;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.T))
        {
            //run the test code
        }
    }
}
