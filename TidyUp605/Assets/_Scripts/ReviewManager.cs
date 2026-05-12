using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class ReviewManager : MonoBehaviour
{
    public static ReviewManager instance;

    //assign in inspector
    [Header("Display variables")]

    //logic stuff
    float timer = 0f;
    public float waitTimeInterval = 1.5f;


    //MISC variables
    int listItemsDisplayed = 0;
    Image currentIMG;


    //overarching Canvas Groups
    public GameObject go_Review;
    public GameObject go_End_continue;
    public GameObject go_Gameplay;

    CanvasGroup Review;
    CanvasGroup End_continue;
    CanvasGroup Gameplay;

    //bool isReview = true;

    //currently a placeholder, add functionality
    public GameObject scoreBar;
    public TMP_Text scoreText;

    //stars 
    public GameObject starOne;
    public GameObject starTwo;
    public GameObject starThree;

    Image img_One;
    Image img_Two;
    Image img_Three;

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

    Image itemIMG;
    Image recepIMG;
    Image rightWrongIMG;

    public ParticleSystem right_sparkles;

    [Header("Logging variables")]
    public List<Tuple<string, string, bool>> ReviewLog = new();


    //debugging only
    //private List<Tuple<string, string, bool>> testLog = new();
    public TMP_Text continueText;
    

    //SETUP
    void Start()
    {
        img_One = starOne.GetComponent<Image>();
        img_Two = starTwo.GetComponent<Image>();
        img_Three = starThree.GetComponent<Image>();

        itemIMG = itemRenderer.GetComponent<Image>();
        recepIMG = receptacleRenderer.GetComponent<Image>();
        rightWrongIMG = rightWrongIcon.GetComponent<Image>();

        Review = go_Review.GetComponent<CanvasGroup>();
        End_continue = go_End_continue.GetComponent<CanvasGroup>();
        Gameplay = go_Gameplay.GetComponent<CanvasGroup>();
 

        if (!Review || !End_continue || !Gameplay)
        {
            Debug.LogWarning("Canvas group not found.");
        }

        Debug.Log("Starting...");
        
        //COMMENT OUT LATER?
        DisplayGameScreen();
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

    void UpdateIcons(Tuple<string, string, bool> tuple)
    {
        //display the right object sprite
        switch (tuple.Item1)
        {
            //ren gaffel
            case "rG":
                itemIMG.sprite = renGaffel;
                break;
            //beskidt gaffel
            case "bG":
                itemIMG.sprite = beskidtGaffel;
                break;

            //ren Tallerken
            case "rT":
                itemIMG.sprite = renTallerken;
                break;
            case "bT":
                itemIMG.sprite = beskidtTallerken;
                break;

            default:
                Debug.Log($"{tuple.Item1} not recognised as object, engaging in fallback...");
                break;
        }

        //display the right receptacle sprite
        switch (tuple.Item2)
        {
            case "skab":
                recepIMG.sprite = skab;
                break;
            case "opvasker":
                recepIMG.sprite = opvasker;
                break;
            case "koele":
                recepIMG.sprite = koele;
                break;
            default:
                Debug.Log($"{tuple.Item2} not recognised as object, engaging in fallback...");
                break;
        }

        if (tuple.Item3)
        {
            rightWrongIMG.sprite = icon_tick;
        }
        else
        {
            rightWrongIMG.sprite = icon_x;
        }

        Debug.Log($"Sprites reset: object is {itemIMG.sprite.name}, recep is {recepIMG.sprite.name}, icon is {rightWrongIMG.sprite.name}.");
        return;
    }


    void DisplayEndScreen()
    {
        Debug.Log("Switching to End screen...");
        Review.alpha = 0;

        Gameplay.alpha = 0;
        Gameplay.interactable = false;

        End_continue.alpha = 1;
        End_continue.interactable = true;
    }

    public void DisplayReviewScreen()
    {
        Debug.Log("Switching to Review screen...");
        End_continue.alpha = 0;
        End_continue.interactable = false;

        Gameplay.alpha = 0;
        Gameplay.interactable = false;

        Review.alpha = 1;
    }

    public void DisplayGameScreen()
    {
        Debug.Log("Switching to Gameplay screen...");
        Review.alpha = 0;

        End_continue.alpha = 0;
        End_continue.interactable = false;

        Gameplay.alpha = 1;
        Gameplay.interactable = true;
    }


    void HideIcons()
    {
        Debug.Log("Hiding icons...");
        itemIMG.color = Color.clear;
        recepIMG.color = Color.clear;
        rightWrongIMG.color = Color.clear;

    }


    //DISPLAY: COROUTINES
     public IEnumerator CRDisplayResults()
     {
        Debug.Log("Running CRDisp...");


        //*disable movement & click
        //*move blackboard forward
        //*sound effect?
        //*narrator: Godt klaret!

        //communicate the total sorts to the StarManager
        //StarManager.instance.totalLevelSorts = GetReviewSequenceTotal();

        //set stars to grey
        img_One.sprite = greyStar;
        img_Two.sprite = greyStar;
        img_Three.sprite = greyStar;


        //make the images transparent for now
        HideIcons();
        DisplayReviewScreen();

        //show review canvas group
        Review.alpha = 1;
        Debug.Log("Showing Review Group...");

        timer = 0f;
        listItemsDisplayed = 0;

        //start the display loop
        StartCoroutine(CRDisplayNextTuple());

        if (ReviewLog.Count == 0)
        {
            Debug.LogWarning("Empty ReviewLog, cannot display!");
        }

        Debug.Log("Finished CRDisplayResults.");
        Debug.Log("Waiting for button click...");
        yield return null; 
     }

    IEnumerator CRDisplayNextTuple()
    {
        Debug.Log("Running CRNextTuple...");

        if (listItemsDisplayed >= ReviewLog.Count)
        {
            Debug.Log("Display done, Exiting loop...");
            ClearBoard();
            DisplayEndScreen();
        }
        else
        {
            Debug.Log($"Attempting to display item {listItemsDisplayed}...");
            UpdateIcons(ReviewLog[listItemsDisplayed]);
            Debug.Log("Attempting to display with delay...");

            //unveil object
            itemIMG.color = Color.white;
            yield return new WaitForSeconds(waitTimeInterval);

            // unveil receptacle
            recepIMG.color = Color.white;
            yield return new WaitForSeconds(waitTimeInterval);

            //unveil right/wrong
            rightWrongIMG.color = Color.white;

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

            yield return new WaitForSeconds(waitTimeInterval);
            HideIcons();

            listItemsDisplayed++;
            yield return null;

            StartCoroutine(CRDisplayNextTuple());
        }

            
    }


































    //OLD THINGS NO LONGER USED
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


    public void DisplayResults(List<Tuple<string, string, bool>> answers)
    {
        //*disable movement & click
        //*move blackboard forward
        //*sound effect?
        //*narrator: Godt klaret!

        //set stars to grey
        img_One.sprite = greyStar;
        img_Two.sprite = greyStar;
        img_Three.sprite = greyStar;


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

    void DisplayIconsOnTimer(bool isRight)
    {
        Debug.Log("Running DisplayIconsOnTimer...");


        //unveil object
        itemIMG.color = Color.white;
        Debug.Log("Object unveiled...");

        //unveil receptacle after a set amount of time
        currentIMG = recepIMG;
        Invoke("UnveilCurrentSprite", waitTimeInterval);
        Debug.Log("Recep unveiled...");

        //unveil right/wrong 
        currentIMG = rightWrongIMG;
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
        timer = 0f;

    }



    void UnveilCurrentSprite()
    {
        Debug.Log($"Unveiling {currentIMG.name} at {timer} seconds elapsed...");
        currentIMG.color = Color.white;
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

    IEnumerator CRDisplayTuple(Tuple<string, string, bool> tuple)
    {
        Debug.Log("Running CRTuple...");

        Debug.Log($"Attempting to display item {listItemsDisplayed}...");
        UpdateIcons(tuple);
        Debug.Log("This happens after UpdateIcons. Attempting to display with delay...");

        //unveil object
        itemIMG.color = Color.white;
        yield return new WaitForSeconds(waitTimeInterval);

        // unveil receptacle
        recepIMG.color = Color.white;
        yield return new WaitForSeconds(waitTimeInterval);

        //unveil right/wrong
        rightWrongIMG.color = Color.white;

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

        yield return new WaitForSeconds(waitTimeInterval);
        HideIcons();

        listItemsDisplayed++;
        yield return true;
    }


    /*
       for (int i = 0; i >= ReviewLog.Count; i++)
       {
           StartCoroutine(CRDisplayTuple(ReviewLog[i]));
           yield return new WaitUntil(() => CRhasFinished);
           Debug.Log($"Moving on to next item...");
       }
       */




}
