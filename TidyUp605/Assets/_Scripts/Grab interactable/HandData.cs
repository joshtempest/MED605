using UnityEngine;

[System.Serializable]
public class HandData 
{
    public FingerData thumb;
    public FingerData index;
    public FingerData middle;
    public FingerData ring;
    public FingerData pinky;
}
    /// <summary>
    /// Proof of concept, this script hold hand data, then there's the  assetmenu built to hold the scriptaable object you maade... it uses the hand data from this script to ..., 
    /// 
    /// min function of these scripts is to make  scriptble objects of hand poses, by setting them once to all objects to then be ble to grab the different objects correctly. 
    /// 
    /// Why? well one could also make a bunch of animations and then put those animations to react on on collision enter of the different objects using their names, this seems to be a better/fster way to scale it.
    /// </summary>
    

    //This class directly just define what a haand is, :3 to define fingers we need to build up the fingers we will be animating, so HandData gets its Finger's data from the public class FingerClass

 
 [System.Serializable]
public class FingerData
{
    public Transform[] phalanges = new Transform[3]; // Each finger (except the thumb) has 3 phalanges.
    public Vector3[] phalanxValues = new Vector3[3];
} 

    // fingers are made up of the phalanxes 3 for each finger, besides the thump that hs 2. 


