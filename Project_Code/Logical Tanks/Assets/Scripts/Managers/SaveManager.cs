using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public static class SaveManager
{
    public static PlayerSaveData PlayerSaveData
    {
        get
        {
            if(_playerSaveData == null)
            {
                Load();
                if(_playerSaveData == null)
                {
                    // First time setup
                    _playerSaveData = new PlayerSaveData();
                    Save();
                }
            }
            return _playerSaveData;
        }
    }

#if UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void SyncFiles();

    [DllImport("__Internal")]
    private static extern void WindowAlert(string message);
#endif
    private static string _fileName = "MJW_Logical_Tanks_SaveData_v1.json";
    private static PlayerSaveData _playerSaveData = null;
    

    public static void Save()
    {
        string savePath = Path.Combine(Application.persistentDataPath, _fileName);
        string saveJson = JsonUtility.ToJson(_playerSaveData);
        using (StreamWriter saveFile = new StreamWriter(savePath))
        {
            saveFile.Write(saveJson);
#if UNITY_WEBGL
            if (Application.platform == RuntimePlatform.WebGLPlayer) { SyncFiles(); }
#endif
        }
    }

    public static void Load()
    {
        string saveJson = "";
        using (StreamReader saveFile = new StreamReader(GetSaveDataPath()))
        {
            saveJson = saveFile.ReadToEnd();
        }
        _playerSaveData = JsonUtility.FromJson<PlayerSaveData>(saveJson);
    }

    public static string GetSaveDataPath()
    {
        return Path.Combine(Application.persistentDataPath, _fileName);
    }
}
