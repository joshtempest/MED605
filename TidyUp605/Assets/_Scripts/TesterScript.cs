using UnityEngine;

public class TesterScript : MonoBehaviour
{
    [SerializeField] string sceneToTest;

    LevelManager levelManager;

    private void Awake()
    {
        levelManager = this.gameObject.GetComponent<LevelManager>();
    }
    private void Start()
    {
        levelManager.currentLevel = sceneToTest;
        levelManager.reloadLevel();
    }
}
