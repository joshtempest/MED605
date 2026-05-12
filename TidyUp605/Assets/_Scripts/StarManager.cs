using UnityEngine;

public class StarManager : MonoBehaviour
{
    public static StarManager instance;

    public int rightSorts = 0;
    public int totalLevelSorts = 0;
    public float rightPercentage = 0f;

    void Awake()
    {
        //Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    void Update()
    {
        rightPercentage = (rightSorts / totalLevelSorts) * 100;
    }
    public void ResetStars()
    {
        totalLevelSorts = 0;
        rightSorts = 0;
        rightPercentage = 0;
    }
    public void addToStarScore(bool isRight)
    {
        totalLevelSorts++;
        if (isRight) totalLevelSorts++;
    }
}
