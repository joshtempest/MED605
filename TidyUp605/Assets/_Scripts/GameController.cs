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
//    Quaternion bbReviewRotation = new Quaternion.Euler(0f, 136.336f, 0f);

    Vector3 bbBackgroundPosition = new (2.52f, 1.939f, 2.503f);
    Quaternion bbBackgroundRotation = Quaternion.identity;
    //make this an animation tomorrow

    bool enoughRightAnswers = false;



    //list to log answers for the review
    private List<Tuple<string, string, bool>> AnswerLog = new();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelManager = this.gameObject.GetComponent<LevelManager>();

        resetScore();

        blackboard = GameObject.Find("Blackboard");
        if (!blackboard)
            Debug.LogWarning("Blackboard not assigned");
        else
        {
            blackboard.SetActive(false);
            blackboard.transform.position = bbBackgroundPosition;
            blackboard.transform.rotation = bbBackgroundRotation;
        }
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

    //reset the answer log
    public void ResetLog()
    {
        AnswerLog = new();
    }

    //log an answer in the list
    public void AddLog(string item, string receptacle, bool isRight)
    {
        AnswerLog.Add(Tuple.Create(item, receptacle, isRight));
        Debug.Log($"Added log: {AnswerLog[AnswerLog.Count].Item1} was placed in {AnswerLog[AnswerLog.Count].Item2}, which was {AnswerLog[AnswerLog.Count].Item3}.");
    }

    public void resetScore()
    {
        rightAnswers = 0;
        wrongAnswers = 0;
        wrongThreshold = 5;
        rightThreshold = 5;
        enoughRightAnswers = false;
        DisplayText.text = "";
    }

    private void Update()
    {
        if (wrongAnswers >= wrongThreshold) 
        {
            enoughRightAnswers = false;
            DisplayResults();
        }
        if (rightAnswers >= rightThreshold)
        {

            enoughRightAnswers = true;
            DisplayResults();

            //det er her vi kan spawne blackboard istedet for bare at gå videre
            
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

    void MoveOrReload(bool successful)
    {
        if (successful)
            levelManager.loadNextLevel();
        else
            levelManager.reloadLevel();
    }

}
