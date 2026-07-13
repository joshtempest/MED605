using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    private LevelManager levelManager;
    public static ToggleBothFarCasters laserManager;

    // Score trackers
    public int rightAnswers;
    public int wrongAnswers;

    // These track exactly how many of each item have been sorted correctly.
    // They start at 0 so the first item dropped brings it to 1, revealing the first hidden object.
    public int plateAnswers = 0;
    public int dirtyPAnswers = 0;
    public int smoerAnswers = 0;
    public int cleanFAnswers = 0;
    public int dirtyFAnswers = 0;

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

    Vector3 bbBackgroundPosition = new(2.66f, 1.735f, 1.557f);
    Vector3 bbBackgroundRotV3 = new(0f, 90f, 0f);
    Quaternion bbBackgroundRotation;

    //bool enoughRightAnswers = false;
    bool levelInProgress = true;
    public bool isVRTut = false;

    // Every time a player sorts an item, the receiver Manager writes it down
    // in this list. The DisplayResults method uses this later to tell the player what they did.
    
    //private List<Tuple<string, string, bool>> AnswerLog = new();

    //G: this functionality was moved into ReviewManager instance


    void Awake()
    {
        bbReviewRotation = Quaternion.Euler(bbReviewRotV3);
        bbBackgroundRotation = Quaternion.Euler(bbBackgroundRotV3);

        levelManager = this.gameObject.GetComponent<LevelManager>();
        if (!levelManager)
        {
            //Debug.LogWarning("OLDDB // LevelManager not assigned");
        }

        // Safety check: Find the blackboard if it wasn't dragged into the inspector manually
        if (blackboard == null)
        {
            blackboard = GameObject.Find("Blackboard");
        }

        if (!blackboard)
        {
            //Debug.Log("OLDDB // Blackboard not assigned! Trying to find it manually...");
            blackboard = GameObject.FindGameObjectWithTag("Blackboard");

            //if (!blackboard)
              //  Debug.LogWarning("OLDDB // Manual search failed, blackboard not assigned.");
        }     
        else
        {
            /*
            blackboard.transform.position = bbBackgroundPosition;
            blackboard.transform.rotation = bbBackgroundRotation;
            */
        }
    }

    private void LocateBlackboard()
    {
        if (!blackboard)
        {
            Debug.Log("Blackboard not assigned! Trying to find it manually...");
            blackboard = GameObject.FindGameObjectWithTag("Blackboard");

            if (!blackboard)
                Debug.LogWarning("Manual search failed, blackboard not assigned.");
        }
    }


    // Changes the score and immediately checks if the player has won.
    public void increaseScore(int score)
    {
        // Prevent score changes if the level is already over
        if (!levelInProgress) return;

        rightAnswers += score;
        //Debug.Log("Score: " + rightAnswers);

        CheckWinCondition();
    }

    // Changes the score and immediately checks if the player has lost
    public void decreaseScore(int score)
    {
        // Prevent score changes if the level is already over
        if (!levelInProgress) return;

        wrongAnswers += score;
        //Debug.Log("Negative Score: " + wrongAnswers);

        CheckLossCondition();
    }
   

    private void CheckWinCondition()
    {
        //Debug.Log("Checking win condition: " + rightAnswers + " right answers, threshold is " + rightThreshold);

        // If the player hit the required amount of right answers
        if (rightAnswers >= rightThreshold)
        {
            levelInProgress = false;
            //enoughRightAnswers = true;

            //Debug.Log("rightThreshold reached. Level end, calling Display");

            //DisplayResults();
            levelManager.loadNextLevel(5); // Move on
        }
    }

    private void CheckLossCondition()
    {
        // If the player hit the limit for mistakes...
        if (wrongAnswers >= wrongThreshold)
        {
            //enoughRightAnswers = false;
            levelInProgress = false;

            if (laserManager != null) laserManager.SetLaserState(true);

            //Debug.Log("wrongThreshold reached. Level end, calling Display");

            //DisplayResults();
            levelManager.reloadLevel(5); // Make them try again
        }
    }


    //Gilah renovations: no right/wrong thresholds, just move on after everything is sorted
    public int totalAnswers;
    public int totalThreshold;

    private void CheckEndCondition()
    {
        if (totalAnswers >= totalThreshold)
        {
            if (isVRTut)
            {
                levelInProgress = false;


                if (laserManager != null) laserManager.SetLaserState(true);

                NewLevelManager.instance.EndVRTraining();
            }
            else
            {
                levelInProgress = false;

                //enable laser - maybe do this later through ReviewManager
                if (laserManager != null) laserManager.SetLaserState(true);

                //Debug.Log("End Condition met, handing off to Review Display management...");

                StartCoroutine(ReviewManager.instance.CRDisplayResults());

            }
        }
    }

    // add to total score and immediately checks if the player has reached level end
    public void addTotalScore(int score)
    {
        // Prevent score changes if the level is already over
        if (!levelInProgress) return;

        totalAnswers += score;
        //Debug.Log("Total Score: " + totalAnswers);

        CheckEndCondition();
    }

    public void resetScore()
    {
        totalAnswers = 0;
        levelInProgress = true;

        LocateBlackboard();
        if (ReviewManager.instance)
        {
            ReviewManager.instance.ResetLog();
        }
        else
            Debug.LogWarning("Cannot locate ReviewManager.instance; cannot reset score.");
        
    }



    /*
    //take this out, old
    public void ResetLog()
    {
        ReviewManager.instance.ResetLog();
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

        rightAnswers = 0;
        wrongAnswers = 0;
        wrongThreshold = 5;

        enoughRightAnswers = false;
        ReviewManager.instance.ResetLog();
        levelInProgress = true;
        if (laserManager != null) laserManager.SetLaserState(false);
    }


    //  Turns on the Blackboard and dynamically types out feedback based on the AnswerLog
    void DisplayResults()
    {
        Debug.Log("Attempting to display results; enoughRightAnswers = " + enoughRightAnswers.ToString());

        if (ButtonText != null)
            ButtonText.text = enoughRightAnswers ? "Gå til næste øvelse" : "Prøv igen";
        else
            Debug.LogWarning("ButtonText is missing! Did you forget to assign it in the Inspector?");

        if (blackboard != null)
            blackboard.SetActive(true);

        if (DisplayText != null)
        {
            // Loop through every single interaction the player had and write a sentence for it
            foreach (Tuple<string, string, bool> log in AnswerLog)
            {
                DisplayText.text += $"{log.Item1} blev placeret i {log.Item2}, som var ";
                DisplayText.text += log.Item3 ? "rigtig." : "forkert. \n";
            }
        }
        else
        {
            Debug.LogWarning("DisplayText is missing! Did you forget to assign it in the Inspector?");
        }
    }

    void MoveOrReload(bool successful)
    {
        if (successful)
            levelManager.loadNextLevel();
        else
            levelManager.reloadLevel();
    }
    */
}