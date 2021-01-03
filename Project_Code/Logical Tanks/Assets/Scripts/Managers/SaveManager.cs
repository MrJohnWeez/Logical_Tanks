using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SyncFiles();

    [DllImport("__Internal")]
    private static extern void WindowAlert(string message);
#endif

    public static SaveManager Instance = null;
    [HideInInspector]
    public PlayerSaveData playerSaveData = null;
    private const string SAVEFILE_NAME = "MJW_Logical_Tanks_SaveData_v1";

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            if(!Load())
            {
                // First time setup
                playerSaveData = new PlayerSaveData();
                Save();
            }
        }
        else { Destroy(this); }
    }

    private static string _fileName
    {
        get
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return String.Format("{0}{1}", SAVEFILE_NAME, ".dat");
#else
            return String.Format("{0}{1}", SAVEFILE_NAME, ".json");
#endif
        }
    }
    
    public void Save()
    {
        string savePath = Path.Combine(Application.persistentDataPath, _fileName);
        string saveJson = JsonUtility.ToJson(playerSaveData);
        using (StreamWriter saveFile = new StreamWriter(savePath))
        {
            saveFile.Write(saveJson);
            Debug.Log("Saved Data");
#if UNITY_WEBGL && !UNITY_EDITOR
            SyncFiles();
#endif
        }
    }

    public bool Load()
    {
        string saveJson = "";
        if (File.Exists(GetSaveDataPath()))
        {
            using (StreamReader saveFile = new StreamReader(GetSaveDataPath()))
            {
                saveJson = saveFile.ReadToEnd();
                Debug.Log("Loaded Data");
            }
            playerSaveData = JsonUtility.FromJson<PlayerSaveData>(saveJson);
            return true;
        }
        return false;
    }

    public void Delete()
    {
        if (File.Exists(GetSaveDataPath()))
        {
            // Makes sure all data is newly generated in case of corrupt files
            File.Delete(GetSaveDataPath());
            Debug.Log("Deleted Data");
            playerSaveData = new PlayerSaveData();
            Save();
        }
    }

    public string GetSaveDataPath() { return Path.Combine(Application.persistentDataPath, _fileName); }

    public void PlatformSafeMessage(string message)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        WindowAlert(message);
#endif
        Debug.Log("WEBGL WINDOW ALERT: " + message);
    }
}
