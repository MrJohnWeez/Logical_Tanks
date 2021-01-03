using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;




/*
What needs saving
-QualityLevel
-LastUnlockedLevel
-MusicLevel
-Levels (List)
    -Level Completed
    -Node Graph (List)
        -Position (Vector3)
        -Connected Out Nodes UUID (list)
*/

[Serializable]
public class PlayerSaveData
{
    public int qualityLevel = 2;
    public int lastLevelUnlocked = 0;
    public int musicLevel = 100;
    
}
