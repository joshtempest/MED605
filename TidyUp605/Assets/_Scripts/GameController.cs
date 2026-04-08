using UnityEngine;

public class GameController : MonoBehaviour
{
    private LevelManager levelManager;

    public int rightAnswer;
    public int wrongAnswer;

    public int threshold;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelManager = this.gameObject.GetComponent<LevelManager>();

        rightAnswer = 0;
        wrongAnswer = 0;
    }

    public void increaseScore(int score)
    {
        rightAnswer += score;
        Debug.Log("Score: " + rightAnswer);
    }
    public void decreaseScore(int score)
    {
        wrongAnswer += score;
        Debug.Log("Negative Score: " + wrongAnswer);
    }

    private void Update()
    {
        if (wrongAnswer > threshold) 
        {
            levelManager.reloadLevel();
        }
    }
}
