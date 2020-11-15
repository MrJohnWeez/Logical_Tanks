using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveData
{
    private const string _levelPrefix = "Level";
    private const string _completePrefix = "Complete";
    private const string _unlockedPrefix = "Unlocked";
    
    public static bool IsLevelComplete(int levelNumber)
    {
        return true;
    }

    public static bool IsLevelUnlocked(int levelNumber)
    {
        return true;
    }

    public static void SetLevelStatus(int levelNumber, bool isUnlocked = false, bool isComplete = false)
    {
        
    }
}
