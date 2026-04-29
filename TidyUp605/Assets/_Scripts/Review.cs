using System;
using System.Collections.Generic; 
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Windows; 

public class Review : MonoBehaviour
{
    //assign in inspector

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

    public Sprite cupboard;
    public Sprite dishwasher;
    public Sprite fridge;

    SpriteRenderer itemSR;
    SpriteRenderer recepSR;
    SpriteRenderer rightwrongSR;


    //debugging only
    private List<Tuple<string, string, bool>> testLog = new();

    void Start()
    {
        SROne = starOne.GetComponent<SpriteRenderer>();
        SRTwo = starTwo.GetComponent<SpriteRenderer>();
        SRThree = starThree.GetComponent<SpriteRenderer>();

        itemSR = itemRenderer.GetComponent<SpriteRenderer>();
        recepSR = receptacleRenderer.GetComponent<SpriteRenderer>();
        rightwrongSR = rightWrongIcon.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.T))
        {
            
        }
    }

    public void ClearBoard()
    {
        Review_start.gameObject.SetActive(false);
        Review_end.gameObject.SetActive(false);
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



        Review_start.gameObject.SetActive(true);
        Review_end.gameObject.SetActive(false);

        for (int i = 0; i < answers.Count; i++)
        {
            
            //display the right object sprite
            switch (answers[i].Item1)
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
                    break;
            }

            //display the right receptacle sprite
            switch (answers[i].Item2)
            {
                case ""
                default:
                    break;
            }
            
                
        }

    }
}
