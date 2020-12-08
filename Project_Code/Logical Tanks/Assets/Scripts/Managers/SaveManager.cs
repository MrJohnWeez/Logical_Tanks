using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class SaveManager
{
#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void SyncFiles();

    [DllImport("__Internal")]
    private static extern void WindowAlert(string message);
#endif

    private static string _fileName = "MJW_Logical_Tanks_SaveData_v1.json";

    public static void Save(PlayerSaveData playerSaveData)
    {
        string savePath = Path.Combine(Application.persistentDataPath, _fileName);
        string saveJson = JsonUtility.ToJson(playerSaveData);
        using (StreamWriter saveFile = new StreamWriter(savePath))
        {
            saveFile.Write(saveJson);
#if UNITY_WEBGL
            if (Application.platform == RuntimePlatform.WebGLPlayer) { SyncFiles(); }
#endif
        }
    }

    public static PlayerSaveData Load()
    {
        string savePath = Path.Combine(Application.persistentDataPath, _fileName);
        string saveJson = "";
        using (StreamReader saveFile = new StreamReader(savePath))
        {
            saveJson = saveFile.ReadToEnd();
        }
        return JsonUtility.FromJson<PlayerSaveData>(saveJson);
    }
}
