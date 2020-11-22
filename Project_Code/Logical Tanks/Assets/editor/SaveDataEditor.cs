using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class SaveDataEditor
{
    [MenuItem("Logical_Tanks/Reset Save Data")]
    public static void ResetSaveData() { PlayerPrefs.DeleteAll(); }
}
