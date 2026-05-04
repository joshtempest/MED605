using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Windows;
using TMPro;
using Unity.VisualScripting;

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

    int listItemsDisplayed = 0;

    SpriteRenderer currentSR;


    //overarching Canvas Groups
    public GameObject go_Review;
    public GameObject go_End_continue;

    CanvasGroup Review;
    CanvasGroup End_continue;

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
    public TMP_Text continueText;

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

        Review = go_Review.GetComponent<CanvasGroup>();
        End_continue = go_End_continue.GetComponent<CanvasGroup>();
 

        if (!Review || !End_continue)
        {
            Debug.LogWarning("Canvas group not found.");
        }

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
        StartCoroutine(CRDisplayResults());

    }

    //placeholder until the rest is coded
    public void DBDisplayResults()
    {
        //move blackboard forward 

        //add items
        for (int i = 0; i <= ReviewLog.Count; i++)
        {
            continueText.text += $"{ReviewLog[i].Item2} -> {ReviewLog[i].Item2} = {ReviewLog[i].Item3}";
        }


    }

    public IEnumerator DBDisplay()
    {
        //add items
        for (int i = 0; i <= ReviewLog.Count; i++)
        {
            continueText.text += $"{ReviewLog[i].Item2} -> {ReviewLog[i].Item2} = {ReviewLog[i].Item3}";
            yield return new WaitForSeconds(waitTimeInterval);
        }



        yield return null;
    }


    //TIMING
    void Update()
    {
        timer += Time.deltaTime;
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
        Review.alpha = 0;
        Review.interactable = false;

        End_continue.alpha = 0;
        End_continue.interactable = false;

        HideIcons();
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

        timer = 0f;
        listItemsDisplayed = 0;
        DisplayNextItem(answers);
    
        //Review_end.gameObject.SetActive(false);
        //Review_start.gameObject.SetActive(true);

        /*
        Debug.Log($"Attempting to display list, length of {answers.Count}...");
        for (int i = 0; i < answers.Count; i++)
        {
            Debug.Log($"Attempting to display item {i}...");
            UpdateIcons(answers[i]);
            Debug.Log("This happens after UpdateIcons. Attempting to run Display...");
            DisplayIconsOnTimer(answers[i].Item3);
        }
        */


        //Review_start.gameObject.SetActive(false);
        //Review_end.gameObject.SetActive(true);

    }

    void DisplayNextItem(List<Tuple<string, string, bool>> answers)
    {
        //exit loop if end of list is reached
        if (listItemsDisplayed >= answers.Count)
        {
            //go to Continue screen
            Debug.Log($"End of list reached: displayed {listItemsDisplayed} of {answers.Count} items.");
            ClearBoard();
            Debug.Log("Entering next phase...");
            continueText.text = "Would you like to continue?";
        }
        else
        {
            Debug.Log($"Attempting to display item {listItemsDisplayed}...");
            UpdateIcons(answers[listItemsDisplayed]);
            Debug.Log("This happens after UpdateIcons. Attempting to run Display...");
            DisplayIconsOnTimer(answers[listItemsDisplayed].Item3);
        }
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



    void DisplayIconsOnTimer(bool isRight)
    {
        Debug.Log("Running DisplayIconsOnTimer...");
        

        //unveil object
        itemSR.color = Color.white;
        Debug.Log("Object unveiled...");

        //unveil receptacle after a set amount of time
        currentSR = recepSR;
        Invoke("UnveilCurrentSprite", waitTimeInterval);
        Debug.Log("Recep unveiled...");

        //unveil right/wrong 
        currentSR = rightWrongSR;
        Invoke("UnveilCurrentSprite", waitTimeInterval);
        Debug.Log("R/W unveiled...");

        if (isRight)
        {
            //add to score
            //sound effects
            //particles
            Debug.Log($"Right answer...");
            right_sparkles.Play();
        }
        else
        {
            //sound effect
            Debug.Log("Wrong answer...");
        }

        Invoke("HideIcons", waitTimeInterval);
        listItemsDisplayed += 1;
        Debug.Log($"Displayed item {listItemsDisplayed}, time elapsed since start is {timer}");
        DisplayNextItem(ReviewLog);

        /* old
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
        */
    }



    void RestartTimer(float delay)
    {
        doContinue = false;
        timer = 0f;
        timerDelay = delay;
    }



    void UnveilCurrentSprite()
    {
        Debug.Log($"Unveiling {currentSR.name} at {timer} seconds elapsed...");
        currentSR.color = Color.white;
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
        Debug.Log("Hiding icons...");
        itemSR.color = Color.clear;
        recepSR.color = Color.clear;
        rightWrongSR.color = Color.clear;

    }


    //DISPLAY: COROUTINE ATTEMPT
    
     IEnumerator CRDisplayResults()
     {
        Debug.Log("Running CRDisp...");
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

        //show review canvas group
        Review.alpha = 1;
        Debug.Log("Showing Review Group...");

        timer = 0f;
        listItemsDisplayed = 0;
        StartCoroutine(CRDisplayNextItem());
        yield return null; 
     }

    IEnumerator CRDisplayNextItem()
    {
        Debug.Log("Running CRNextItem...");
        //exit loop if end of list is reached
        if (listItemsDisplayed >= ReviewLog.Count)
        {
            //go to Continue screen
            Debug.Log($"End of list reached: displayed {listItemsDisplayed} of {ReviewLog.Count} items.");
            ClearBoard();
            Debug.Log("Entering next phase...");
            continueText.text = "Would you like to continue?";
        }
        else
        {
            Debug.Log($"Attempting to display item {listItemsDisplayed}...");
            UpdateIcons(ReviewLog[listItemsDisplayed]);
            Debug.Log("This happens after UpdateIcons. Attempting to display with delay...");

            //unveil object
            itemSR.color = Color.white;
            yield return new WaitForSeconds(waitTimeInterval);

            // unveil receptacle
            recepSR.color = Color.white;
            yield return new WaitForSeconds(waitTimeInterval);

            //unveil right/wrong
            rightWrongSR.color = Color.white;

            //if it was right
            if (ReviewLog[listItemsDisplayed].Item3)
            {
                right_sparkles.Play();
                //add audio effect

                //add score to score bar

                Debug.Log($"Item {listItemsDisplayed} was right!");

            }
            else
            {
                //add sound effect
                //add narrator

                Debug.Log($"Item {listItemsDisplayed} was wrong! :(");
            }

            Debug.Log($"Display of item {listItemsDisplayed} complete, {timer} seconds elapsed.");
            listItemsDisplayed++;

            StartCoroutine(CRDisplayNextItem());
        }
    }

    
}
