﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveData
{
    public static int LastUnlockedLevel
    {
        get { return PlayerPrefs.GetInt(LAST_UNLOCKED_LEVEL_KEY, 0); }
        set { PlayerPrefs.SetInt(LAST_UNLOCKED_LEVEL_KEY, value); }
    }

    private const string LEVEL_PREFIX = "Level_";
    private const string COMPLETED_POSTFIX = "_Complete";
    private const string LAST_UNLOCKED_LEVEL_KEY = "Last_Level_Unlocked";
    
    public static bool IsLevelComplete(int levelNumber)
    {
        string key = string.Format("{0}_{1}_{2}", LEVEL_PREFIX, levelNumber, COMPLETED_POSTFIX);
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    public static bool IsLevelLocked(int levelNumber) { return levelNumber > LastUnlockedLevel; }

    public static void SetLevelComplete(int levelNumber, bool isComplete = false)
    {
        string key = string.Format("{0}_{1}_{2}", LEVEL_PREFIX, levelNumber, COMPLETED_POSTFIX);
        PlayerPrefs.SetInt(key, isComplete ? 1 : 0);
    }

    public static void ResetGameData() { PlayerPrefs.DeleteAll(); }
    public static void SaveGameData() { PlayerPrefs.Save(); }
    
    public static void UnlockAllLevels()
    {
        LastUnlockedLevel = 20;
        PlayerPrefs.Save();
    }
}