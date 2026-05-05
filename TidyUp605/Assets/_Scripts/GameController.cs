using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro.EditorUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    private LevelManager levelManager;
    public ToggleBothFarCasters laserManager;

    public int rightAnswers;
    public int wrongAnswers;

    public int plateAnswers = -1;
    public int dirtyPAnswers = -1;
    public int smoerAnswers = -1;

    public int wrongThreshold = 5;
    public int rightThreshold = 5;

    //variables for review animations; assign in inspector
    public GameObject blackboard;
    public TMP_Text DisplayText;
    public TMP_Text ButtonText;

    private string test;

    Vector3 bbReviewPosition = new(-0.59f, 1.939f, 2.48f);
    Vector3 bbReviewRotV3 = new(0f, 136.336f, 0f);
    Quaternion bbReviewRotation;

    Vector3 bbBackgroundPosition = new(2.52f, 1.939f, 2.503f);
    Vector3 bbBackgroundRotV3 = new(0f, 90f, 0f);
    Quaternion bbBackgroundRotation;

    bool enoughRightAnswers = false;
    bool levelInProgress = true;

    //list to log answers for the review
    private List<Tuple<string, string, bool>> AnswerLog = new();


    // Changed Start() to Awake() so this script initializes BEFORE LevelManager's Start() runs
    void Awake()
    {
        bbReviewRotation = Quaternion.Euler(bbReviewRotV3);
        bbBackgroundRotation = Quaternion.Euler(bbBackgroundRotV3);

        levelManager = this.gameObject.GetComponent<LevelManager>();
        if (!levelManager)
        {
            Debug.LogWarning("LevelManager not assigned");
        }

        blackboard = GameObject.Find("Blackboard");
        if (!blackboard)
            Debug.LogWarning("Blackboard not assigned");
        else
        {
            blackboard.transform.position = bbBackgroundPosition;
            blackboard.transform.rotation = bbBackgroundRotation;
        }
    }

    void Start()
    {
        //resetScore();
    }

    public void increaseScore(int score)
    {
        // Prevent score changes if the level is already over
        if (!levelInProgress) return;

        rightAnswers += score;
        Debug.Log("Score: " + rightAnswers);

        CheckWinCondition();
    }

    public void decreaseScore(int score)
    {
        // Prevent score changes if the level is already over
        if (!levelInProgress) return;

        wrongAnswers += score;
        Debug.Log("Negative Score: " + wrongAnswers);

        CheckLossCondition();
    }

    private void CheckWinCondition()
    {
        Debug.Log("Checking win condition: " + rightAnswers + " right answers, threshold is " + rightThreshold);
        if (rightAnswers >= rightThreshold)
        {
            levelInProgress = false;
            enoughRightAnswers = true;

            Debug.Log("rightThreshold reached. Level end, calling Display");

            DisplayResults();
            levelManager.loadNextLevel(5);
        }
    }

    private void CheckLossCondition()
    {
        if (wrongAnswers >= wrongThreshold)
        {
            enoughRightAnswers = false;
            levelInProgress = false;

            if (laserManager != null) laserManager.SetLaserState(true);

            Debug.Log("wrongThreshold reached. Level end, calling Display");

            DisplayResults();
            levelManager.reloadLevel(5);
        }
    }

    public void ResetLog()
    {
        AnswerLog = new();
    }

    public void AddLog(string item, string receptacle, bool isRight)
    {
        AnswerLog.Add(Tuple.Create(item, receptacle, isRight));
        Debug.Log($"Added log: {AnswerLog[AnswerLog.Count - 1].Item1} was placed in {AnswerLog[AnswerLog.Count - 1].Item2}, which was {AnswerLog[AnswerLog.Count - 1].Item3}.");
    }

    public void resetScore()
    {
        if (levelManager == null)
        {
            levelManager = this.gameObject.GetComponent<LevelManager>();
        }

        test = levelManager.currentLevel;
        Debug.Log("Resetting score..." + levelManager.currentLevel);
        //Debug.Log(test);
        rightAnswers = 0;
        wrongAnswers = 0;

        wrongThreshold = 5;

        enoughRightAnswers = false;
        ResetLog();
        levelInProgress = true;
        if (laserManager != null) laserManager.SetLaserState(false);
    }

    void DisplayResults()
    {
        Debug.Log("Attempting to display results; enoughRightAnswers = " + enoughRightAnswers.ToString());

        ButtonText.text = enoughRightAnswers ? "Gå til næste øvelse" : "Prøv igen";

        blackboard.SetActive(true);

        foreach (Tuple<string, string, bool> log in AnswerLog)
        {
            DisplayText.text += $"{log.Item1} blev placeret i {log.Item2}, som var ";
            DisplayText.text += log.Item3 ? "rigtig." : "forkert. \n";
        }
    }

    void MoveOrReload(bool successful)
    {
        if (successful)
            levelManager.loadNextLevel();
        else
            levelManager.reloadLevel();
    }
}