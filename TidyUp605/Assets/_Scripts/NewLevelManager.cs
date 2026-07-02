using UnityEngine;

public class NewLevelManager : MonoBehaviour
{
    //make a singleton!

    void Annihilation()
    {
        //Debug.Log("Annihilation initiated");
        //LogData.instance.AddToLogs("Annihalating...");

        ///Finds all objects with the specified tags and destroys them, to clear the scene for the new level.
        ///Receptacles need to be cleared, to insure that only the needed receptacles for the level are present.
        GameObject[] receptables = GameObject.FindGameObjectsWithTag("receptacle");
        for (int i = 0; i < receptables.Length; i++)
        {
            Destroy(receptables[i]);
        }
        GameObject[] mad = GameObject.FindGameObjectsWithTag("Mad");
        for (int i = 0; i < mad.Length; i++)
        {
            Destroy(mad[i]);
        }
        GameObject[] service = GameObject.FindGameObjectsWithTag("Service");
        for (int i = 0; i < service.Length; i++)
        {
            Destroy(service[i]);
        }
        GameObject[] beskidt = GameObject.FindGameObjectsWithTag("Beskidt");
        for (int i = 0; i < beskidt.Length; i++)
        {
            Destroy(beskidt[i]);
        }

        if (gameController != null)
        {
            gameController.resetScore();
        }
        else
        {
            Debug.LogWarning("GameController is missing on " + gameObject.name + ". Cannot reset score.");
        }
    }
    public void loadCustomSeq1(int cleanTallerken, int dirtyTallerken, bool boolskab, bool boolopvask, bool boolkoele, int threshold)
    {
        //compareScene("Tutorial_Practice");
        Annihilation();

        if (boolskab) { Instantiate(skab, skabPlatformPos, skabPlatform.transform.rotation); }
        if (boolopvask) { Instantiate(opvaskemaskine, opvaskemaskinePlatformPos, opvaskemaskinePlatform.transform.rotation); }
        if (boolkoele) { Instantiate(koeleskab, koelePlatformPos, koelePlatform.transform.rotation); }

        for (int i = 0; i < cleanTallerken; i++)
        {
            spawnerScript.spawnThisObject("rT");
        }
        for (int i = 0; i < dirtyTallerken; i++)
        {
            spawnerScript.spawnThisObject("bT");
        }
        gameController.totalThreshold = threshold;
    }
}

public class LevelData
{

}
