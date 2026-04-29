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

    public int wrongThreshold = 5;
    public int rightThreshold = 5;

    //variables for review animations; assign in inspector
    public GameObject blackboard;
    public TMP_Text DisplayText;
    public TMP_Text ButtonText;
    
    //figure out later
    /*
    public Image item_img;
    public Image receptacle_img;

    public Sprite cleanPlate;
    public Sprite dirtyPlate;
    public Sprite butter;

    public Sprite cupboard;
    public Sprite dishwasher;
    public Sprite fridge;

    //particle systems
    public GameObject right_tick;
    public GameObject wrong_x;
    */

    Vector3 bbReviewPosition = new (-0.59f,1.939f,2.48f);
    Vector3 bbReviewRotV3 = new (0f, 136.336f, 0f);
    Quaternion bbReviewRotation;

    Vector3 bbBackgroundPosition = new (2.52f, 1.939f, 2.503f);
    Vector3 bbBackgroundRotV3 = new(0f, 90f, 0f);
    Quaternion bbBackgroundRotation;
    //make this an animation tomorrow

    bool enoughRightAnswers = false;
    bool levelInProgress = true;



    //list to log answers for the review
    private List<Tuple<string, string, bool>> AnswerLog = new();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bbReviewRotation = Quaternion.Euler(bbReviewRotV3);
        bbBackgroundRotation = Quaternion.Euler(bbBackgroundRotV3);

        levelManager = this.gameObject.GetComponent<LevelManager>();
        if (!levelManager) { 
            Debug.LogWarning("LevelManager not assigned");
        }
        resetScore();

        blackboard = GameObject.Find("Blackboard");
        if (!blackboard)
            Debug.LogWarning("Blackboard not assigned");
        else
        {
            //blackboard.SetActive(false);
            blackboard.transform.position = bbBackgroundPosition;
            blackboard.transform.rotation = bbBackgroundRotation;
        }

    }

    public void increaseScore(int score)
    {
        rightAnswers += score;
        Debug.Log("Score: " + rightAnswers);
        LogData.instance.AddToLogs("Score: " + rightAnswers);
    }
    public void decreaseScore(int score)
    {
        wrongAnswers += score;
        Debug.Log("Negative Score: " + wrongAnswers);
        LogData.instance.AddToLogs("Negative Score: " + wrongAnswers);
    }

    //reset the answer log
    public void ResetLog()
    {
        AnswerLog = new();
    }

    //log an answer in the list
    public void AddLog(string item, string receptacle, bool isRight)
    {
        AnswerLog.Add(Tuple.Create(item, receptacle, isRight));
        //Debug.Log($"List is {AnswerLog.Count} items long.");
        Debug.Log($"Added log: {AnswerLog[AnswerLog.Count - 1].Item1} was placed in {AnswerLog[AnswerLog.Count - 1].Item2}, which was {AnswerLog[AnswerLog.Count - 1].Item3}.");
        LogData.instance.AddToLogs($"Added log: {AnswerLog[AnswerLog.Count - 1].Item1} was placed in {AnswerLog[AnswerLog.Count - 1].Item2}, the success of which was {AnswerLog[AnswerLog.Count - 1].Item3}.");
    }

    public void resetScore()
    {
        Debug.Log("Resetting score..." + levelManager.currentLevel);
        rightAnswers = 0;
        wrongAnswers = 0;

        //placeholder/default numbers; are updated in the LoadLevel functions in LevelManagers
        wrongThreshold = 5;
        rightThreshold = 5;


        enoughRightAnswers = false;
        ResetLog();
        levelInProgress = true;
        if (laserManager != null) laserManager.SetLaserState(false);
        DisplayText.text = $"Current level is {levelManager.currentLevel} \n";
        //LogData.instance.AddToLogs($"Resetting... current level is {levelManager.currentLevel}.");
    }

    private void Update()
    {
        if (levelInProgress)
        {
            if (wrongAnswers >= wrongThreshold)
            {
                enoughRightAnswers = false;
                levelInProgress = false;
                if (laserManager != null) laserManager.SetLaserState(true);

                Debug.Log("wrongThreshold reached. Level end, calling Display");
                LogData.instance.AddToLogs("wrongThreshold reached. Level end, calling Display");
               

                //add a review in Display
                DisplayResults();

                //change to button later
                levelManager.reloadLevel(5);
            }
            if (rightAnswers >= rightThreshold)
            {

                levelInProgress = false;
                enoughRightAnswers = true;

                Debug.Log("rightThreshold reached. Level end, calling Display");
                LogData.instance.AddToLogs("rightThreshold reached. Level end, calling Display");

                //add blackboard review
                DisplayResults();

                //change to button later
                levelManager.loadNextLevel(5);
            }
        }
        
    }

    void DisplayResults()
    {
        /*
        //move blackboard to be very visible //Movement comes later
        blackboard.transform.position = bbReviewPosition;
        //blackboard.transform.rotation = bbReviewPosition;
        //REPLACE WITH ANIMATION CLIP
        */


        Debug.Log("Attempting to display results; enoughRightAnswers = " + enoughRightAnswers.ToString());
        LogData.instance.AddToLogs("Attempting to display results; enoughRightAnswers = " + enoughRightAnswers.ToString());

        ButtonText.text = enoughRightAnswers ? "Gå til næste øvelse" : "Prøv igen";

        blackboard.SetActive(true);

        foreach (Tuple<string, string, bool> log in AnswerLog)
        {
            DisplayText.text += $"{log.Item1} blev placeret i {log.Item2}, som var ";
            DisplayText.text += log.Item3 ? "rigtig." : "forkert. \n";

            /*
            if (!log.Item3)
            {
                switch (log.Item1) 
                {
                    case 
                }
                    
                DisplayText.text += "\n The correct receptacle would be "; 
            }
            */
        }

        /*
        //UNDER CONSTRUCTION
        foreach (Tuple<string, string, bool> log in AnswerLog)
        {

            //if it was correct
            if (log.Item3)
            {

            }
        }

       //if successful:
        //display correct item(s) moving into correct receptacle(s)
        //dislay positive particles 
        //play positive sound

        //display button; text: "gå til næste øvelse"

        //if unsuccessful: 
        //display error (wrong item moving into wrong receptacle)
        //display the right alternative (right receptacle
        //repeat for all mistakes
        //display button, text: "prøv igen!"
        */

    }

    //under construction
    void MoveOrReload(bool successful)
    {
        if (successful)
            levelManager.loadNextLevel();
        else
            levelManager.reloadLevel();
    }

}
