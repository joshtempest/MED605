using UnityEngine;

public class GameController : MonoBehaviour
{
    private LevelManager levelManager;

    public int rightAnswers;
    public int wrongAnswers;

    public int threshold;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelManager = this.gameObject.GetComponent<LevelManager>();

        resetScore();
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
    }

    private void Update()
    {
        if (wrongAnswers >= threshold) 
        {
            levelManager.reloadLevel();
        }
    }
}
