using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Windows;
using TMPro;

public class ReviewManager : MonoBehaviour
{
    public static ReviewManager instance;

    //assign in inspector
    [Header("Display variables")]

    //logic stuff
    float timer = 0f;
    float timerDelay = 0f;
    public float waitTimeInterval = 3f;
    bool doContinue = true;

    //overarching Canvas
    public Canvas Review_start;
    public Canvas Review_end;

    //currently a placeholder, add functionality
    public GameObject scoreBar;

    //stars 
    public GameObject starOne;
    public GameObject starTwo;
    public GameObject starThree;

    SpriteRenderer SROne;
    SpriteRenderer SRTwo;
    SpriteRenderer SRThree;

    public Sprite yellowStar;
    public Sprite greyStar;

    //log display (=showing what they did right/wrong)
    public GameObject itemRenderer;
    public GameObject receptacleRenderer;
    public GameObject rightWrongIcon;

    public Sprite renGaffel;
    public Sprite beskidtGaffel;
    public Sprite renTallerken;
    public Sprite beskidtTallerken;

    public Sprite skab;
    public Sprite opvasker;
    public Sprite koele;

    public Sprite icon_x;
    public Sprite icon_tick;

    SpriteRenderer itemSR;
    SpriteRenderer recepSR;
    SpriteRenderer rightWrongSR;

    public ParticleSystem right_sparkles;

    [Header("Logging variables")]
    public List<Tuple<string, string, bool>> ReviewLog = new();


    //debugging only
    //private List<Tuple<string, string, bool>> testLog = new();
    public TMP_Text debugText; 

    //SETUP
    private void Awake()
    {
        /*
        // Singleton Pattern
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
        */
    }

    void Start()
    {
        SROne = starOne.GetComponent<SpriteRenderer>();
        SRTwo = starTwo.GetComponent<SpriteRenderer>();
        SRThree = starThree.GetComponent<SpriteRenderer>();

        itemSR = itemRenderer.GetComponent<SpriteRenderer>();
        recepSR = receptacleRenderer.GetComponent<SpriteRenderer>();
        rightWrongSR = rightWrongIcon.GetComponent<SpriteRenderer>();

        Debug.Log("Starting...");
    }

    //DEBUG
    public void DBAdd()
    {
        Debug.Log("Adding items...");
        AddLog("rG", "skab", true);
        AddLog("bG", "skab", false);
        AddLog("rT", "koele", false);
    }

    public void DBStart()
    {
        Debug.Log("Attempting to display...");
        DisplayResults(ReviewLog);
    }

    //TIMING
    void Update()
    {
        timer += Time.deltaTime;

        if (timer < waitTimeInterval)
        {
            doContinue = false;
        }
        else
        {
            doContinue = true;
            timer = 0f;
        }
    }


    //LOG MANAGEMENT
    public void AddLog(string item, string receptacle, bool isRight)
    {
        ReviewLog.Add(Tuple.Create(item, receptacle, isRight));
        Debug.Log($"Added log: {ReviewLog[ReviewLog.Count - 1].Item1} was placed in {ReviewLog[ReviewLog.Count - 1].Item2}, which was {ReviewLog[ReviewLog.Count - 1].Item3}.");
        Debug.Log($"List is {ReviewLog.Count} items long.");
    }

    public void ResetLog()
    {
        ReviewLog = new();
        Debug.Log("ReviewLog reset.");
    }

    //DISPLAY MANAGEMENT
    public void ClearBoard()
    {
        //Review_start.gameObject.SetActive(false);
        //Review_end.gameObject.SetActive(false);
    }

    public void DisplayResults(List<Tuple<string, string, bool>> answers)
    {
        //*disable movement & click
        //*move blackboard forward
        //*sound effect?
        //*narrator: Godt klaret!

        //set stars to grey
        SROne.sprite = greyStar;
        SRTwo.sprite = greyStar;
        SRThree.sprite = greyStar;


        //make the sprite renderers transparent for now
        HideIcons();
    
        //Review_end.gameObject.SetActive(false);
        //Review_start.gameObject.SetActive(true);

        Debug.Log($"Attempting to display list, length of {answers.Count}...");
        for (int i = 0; i < answers.Count; i++)
        {
            Debug.Log($"Attempting to display item {i}...");
            UpdateIcons(answers[i]);
            Debug.Log("This happens after UpdateIcons. Attempting to run Display...");
            DisplayIconsAndWait(answers[i].Item3);
        }
        Debug.Log("Entering next phase...");

        //Review_start.gameObject.SetActive(false);
        //Review_end.gameObject.SetActive(true);

    }

    void UpdateIcons(Tuple<string, string, bool> tuple)
    {
        //display the right object sprite
        switch (tuple.Item1)
        {
            //ren gaffel
            case "rG":
                itemSR.sprite = renGaffel;
                break;
            //beskidt gaffel
            case "bG":
                itemSR.sprite = beskidtGaffel;
                break;

            //ren Tallerken
            case "rT":
                itemSR.sprite = renTallerken;
                break;
            case "bT":
                itemSR.sprite = beskidtTallerken;
                break;

            default:
                Debug.Log($"{tuple.Item1} not recognised as object, engaging in fallback...");
                break;
        }

        //display the right receptacle sprite
        switch (tuple.Item2)
        {
            case "skab":
                recepSR.sprite = skab;
                break;
            case "opvasker":
                recepSR.sprite = opvasker;
                break;
            case "koele":
                recepSR.sprite = koele;
                break;
            default:
                Debug.Log($"{tuple.Item2} not recognised as object, engaging in fallback...");
                break;
        }

        if (tuple.Item3)
        {
            rightWrongSR.sprite = icon_tick;
        }
        else
        {
            rightWrongSR.sprite = icon_x;
        }

        Debug.Log($"Sprites reset: object is {itemSR.sprite.name}, recep is {recepSR.sprite.name}, icon is {rightWrongSR.sprite.name}.");
        return;
    }
    void DisplayIconsAndWait(bool isRight)
    {
        Debug.Log("Running DisplayIconsAndWait...");
        

        //unveil object
        itemSR.color = Color.white;
        Debug.Log("Object unveiled...");

        //wait for timer
        RestartTimer(waitTimeInterval);
        if (doContinue)
        {
            RestartTimer(waitTimeInterval);

            //unveil receptacle
            recepSR.color = Color.white;
            Debug.Log("Recep unveiled...");


            if (doContinue)
            {
                RestartTimer(waitTimeInterval * 2);

                //unveil right/wrong, add effects
                rightWrongSR.color = Color.white;
                Debug.Log("RW unveiled...");

                if (isRight)
                {
                    //*playSoundEffect
                    right_sparkles.Play();
                    //add to score
                    //update score bar
                }
                else
                {
                    //*playSoundEffect
                }

                if (doContinue)
                {
                    Debug.Log("Attempting to move to next item...");
                    HideIcons();
                }
            }
        }
    }

    void RestartTimer(float delay)
    {
        doContinue = false;
        timer = 0f;
        timerDelay = delay;
    }



    /* OLD
    public IEnumerator DisplayIconsAndWait(bool answerIsRight)
    {
        

        //unveil object
        itemSR.color = Color.white;
        yield return new WaitForSeconds(waitTimeInterval);

        //unveil receptacle
        recepSR.color = Color.white;
        yield return new WaitForSeconds(waitTimeInterval);

        //unveil rightness 
        rightWrongSR.color = Color.white;
        //*play sound effect

        if (answerIsRight)
        {
            //*playSoundEffect
            right_sparkles.Play();
            //add to score
            //update score bar

            yield return new WaitForSeconds(waitTimeInterval * 2);
        }
        else
        {
            yield return new WaitForSeconds(waitTimeInterval);
        }

        HideIcons();
        yield break;
    }
    */

    void HideIcons()
    {
        itemSR.color = Color.clear;
        recepSR.color = Color.clear;
        rightWrongSR.color = Color.clear;

    }
}
