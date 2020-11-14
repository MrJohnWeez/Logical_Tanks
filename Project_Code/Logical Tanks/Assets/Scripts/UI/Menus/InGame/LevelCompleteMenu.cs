using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelCompleteMenu : BaseInGameMenu
{
    [SerializeField] private TMP_Text _title = null;

    protected override void Awake()
    {
        base.Awake();
        _title.text = string.Format("Level {0} Completed!", gameManager.LevelNumber + 1);
    }

    protected override void CloseMenu() { gameManager.ToLevelSelection(); }
}
