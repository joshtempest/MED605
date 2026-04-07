using UnityEngine;

public class GameController : MonoBehaviour
{
    public int rightAnswer;
    public int wrongAnswer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
}
