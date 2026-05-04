using UnityEngine;

public class StarManager : MonoBehaviour
{
    public static StarManager instance;
    
    public int rightSorts = 0;

    /// <summary>
    /// max sorts in the current segment
    /// </summary>
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
}
