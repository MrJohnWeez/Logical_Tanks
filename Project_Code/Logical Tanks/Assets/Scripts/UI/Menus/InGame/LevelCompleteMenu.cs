using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCompleteMenu : BaseInGameMenu
{
    [SerializeField] private TMP_Text _title = null;

    protected override void Awake()
    {
        base.Awake();
        _title.text = string.Format("Level {0} Completed!", gameManager.LevelNumber + 1);
    }

    private void Start()
    {
        SaveData.SetLevelComplete(gameManager.LevelNumber, true);
        if(SaveData.LastUnlockedLevel < gameManager.LevelNumber + 1)
        {
            SaveData.LastUnlockedLevel = gameManager.LevelNumber + 1;
        }
        SaveData.SaveGameData();
    }
    protected override void CloseMenu() { gameManager.ToLevelSelection(); }
}
