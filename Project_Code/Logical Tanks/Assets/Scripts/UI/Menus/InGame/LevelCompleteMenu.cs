using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCompleteMenu : BaseInGameMenu
{
    [SerializeField] private TMP_Text _title = null;
    [SerializeField] private GameObject _finalWinMenu = null;
    private const int FINAL_LEVEL = 16;

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

        if(gameManager.LevelNumber == FINAL_LEVEL)
        {
            SpawnMenu(_finalWinMenu);
        }
    }

    protected override void CloseMenu() { gameManager.ToLevelSelection(); }
}
