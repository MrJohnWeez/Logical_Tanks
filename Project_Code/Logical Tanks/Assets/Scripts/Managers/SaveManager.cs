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
            if (_playerSaveData == null)
            {
                Load();
                if (_playerSaveData == null)
                {
                    // First time setup
                    _playerSaveData = new PlayerSaveData();
                    Save();
                }
            }
            return _playerSaveData;
        }
    }
    
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SyncFiles();

    [DllImport("__Internal")]
    private static extern void WindowAlert(string message);
#endif
    private static string _fileName
    {
        get
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return "MJW_Logical_Tanks_SaveData_v1.dat";
#else
            return "MJW_Logical_Tanks_SaveData_v1.json";
#endif
        }
    }
    private static PlayerSaveData _playerSaveData = null;

    public static void Save()
    {
        string savePath = Path.Combine(Application.persistentDataPath, _fileName);
        string saveJson = JsonUtility.ToJson(_playerSaveData);
        using (StreamWriter saveFile = new StreamWriter(savePath))
        {
            saveFile.Write(saveJson);
#if UNITY_WEBGL && !UNITY_EDITOR
            SyncFiles();
#endif
        }
    }

    public static void Load()
    {
        string saveJson = "";
        if (File.Exists(GetSaveDataPath()))
        {
            using (StreamReader saveFile = new StreamReader(GetSaveDataPath()))
            {
                saveJson = saveFile.ReadToEnd();
            }
            _playerSaveData = JsonUtility.FromJson<PlayerSaveData>(saveJson);
        }
    }

    public static void Delete()
    {
        if (File.Exists(GetSaveDataPath()))
        {
            // Makes sure all data is newly generated in case of corrupt files
            File.Delete(GetSaveDataPath());
            _playerSaveData = new PlayerSaveData();
            Save();
        }
    }

    public static string GetSaveDataPath()
    {
        return Path.Combine(Application.persistentDataPath, _fileName);
    }

    public static void PlatformSafeMessage(string message)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
            WindowAlert(message);
#endif

        Debug.Log(message);
    }
}
