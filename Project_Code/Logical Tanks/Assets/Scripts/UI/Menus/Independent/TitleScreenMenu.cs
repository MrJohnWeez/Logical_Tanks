using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenMenu : BaseMenu
{
    [Header("TitleScreenMenu")]
    [SerializeField] private Button _playButton = null;
    [SerializeField] private Button _settingsButton = null;
    [SerializeField] private Button _aboutButton = null;
    [SerializeField] private GameObject _levelSelectionMenu = null;
    [SerializeField] private GameObject _settingsMenu = null;
    [SerializeField] private GameObject _aboutMenu = null;

    protected override void Awake()
    {
        base.Awake();
        _playButton.onClick.AddListener(() => SpawnMenu(_levelSelectionMenu));
        _settingsButton.onClick.AddListener(() => SpawnMenu(_settingsMenu));
        _aboutButton.onClick.AddListener(() => SpawnMenu(_aboutMenu));
    }

    public void ForceOpenLevelMenu() { SpawnMenu(_levelSelectionMenu); }
}
