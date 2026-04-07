using UnityEngine;

public class GameController : MonoBehaviour
{
    public int ScoreValue;
    public int negativeScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScoreValue = 0;
        negativeScore = 0;
    }

    public void increaseScore(int score)
    {
        ScoreValue += score;
        Debug.Log("Score: " + ScoreValue);
    }
    public void decreaseScore(int score)
    {
        negativeScore += score;
        Debug.Log("Negative Score: " + negativeScore);
    }
}
