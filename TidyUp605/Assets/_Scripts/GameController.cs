using UnityEngine;

public class GameController : MonoBehaviour
{
    private LevelManager levelManager;

    public int rightAnswers;
    public int wrongAnswers;

    public int wrongThreshold = 5;
    public int rightThreshold = 5;

    public GameObject blackboard;
    public Vector3 bbReviewPosition;
    public Vector3 bbBackgroundPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelManager = this.gameObject.GetComponent<LevelManager>();
       
        resetScore();
        blackboard.SetActive(false);
    }

    public void increaseScore(int score)
    {
        rightAnswers += score;
        Debug.Log("Score: " + rightAnswers);
    }
    public void decreaseScore(int score)
    {
        wrongAnswers += score;
        Debug.Log("Negative Score: " + wrongAnswers);
    }
    public void resetScore()
    {
        rightAnswers = 0;
        wrongAnswers = 0;
        wrongThreshold = 5;
        rightThreshold = 5;
    }

    private void Update()
    {
        if (wrongAnswers >= wrongThreshold) 
        {
            levelManager.reloadLevel();
        }
        if (rightAnswers >= rightThreshold)
        {
            //det er her vi kan spawne blackboard istedet for bare at gå videre
            levelManager.loadNextLevel();
        }
    }
}
