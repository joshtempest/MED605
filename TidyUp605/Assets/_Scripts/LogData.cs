using System;
using System.IO;
using System.Globalization;
using UnityEngine;

public class LogData : MonoBehaviour
{
    public static LogData instance;

    public static int numberOfLogs = 0;

    private string _dataPath;
    private string _currentTxtFile;
    private string _persistentLogFile;


    private void Awake()
    {
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

        //get the persistent data path
        _dataPath = Application.persistentDataPath + "/Logs/" ;

        Debug.Log(_dataPath);

        _currentTxtFile = CreateNewLogFile();
        _persistentLogFile = _dataPath + "PersistentLogs.txt";

        Initialise();

    }

    void Initialise()
    {
        //initialise directory
        NewDirectory();
        NewTextFile();
        TestLogging();
    }

    void CheckFormatPaths()
    {
        Debug.LogFormat("Path separator character: {0}", Path.PathSeparator);
        Debug.LogFormat("Directory separator character: {0}", Path.DirectorySeparatorChar);
        Debug.LogFormat("Current directory: {0}", Directory.GetCurrentDirectory());
        Debug.LogFormat("Temporary path: {0}", Path.GetTempPath());
    }

    //Ferrone
    public void NewDirectory()
    {
        // 1
        if (Directory.Exists(_dataPath))
        {
            // 2
            Debug.Log("Directory already exists...");
            return;
        }
        // 3
        Directory.CreateDirectory(_dataPath);
        Debug.Log("New directory created!");
    }
    string CreateNewLogFile()
    {
        DateTime today = DateTime.Now;
        string date = today.Date.ToString();
        Debug.Log($"date: {date}");
        
        string txtPath = $"{_dataPath}Log_{today.Year}_{today.Month}_{today.Day}_{numberOfLogs}.txt";
        numberOfLogs += 1;

        Debug.Log(txtPath);

        return txtPath;
    }

    //ferrone
    public void NewTextFile()
    {
        // 1
        if (File.Exists(_persistentLogFile))
        {
            Debug.Log("File already exists...");
            return;
        }
        // 2
        File.WriteAllText(_persistentLogFile, "<PLAYTEST LOG>\n\n");
        // 3
        Debug.Log("New file created!");
    }

    //partially Ferrone 
    public void AddToLogs(string addition)
    {
        if (!File.Exists(_persistentLogFile))
        {
            Debug.Log("File doesn't exist...");
            return;
        }
        
        File.AppendAllText(_persistentLogFile, $"\n{DateTime.Now} -- {addition}\n");
        
        Debug.Log("File updated successfully!");
    }
    
    public void TestLogging()
    {
        AddToLogs("helloooooo");
        AddToLogs("jfsalkj2jo1498749879878347989u");
        AddToLogs("nouple");
    }

}
