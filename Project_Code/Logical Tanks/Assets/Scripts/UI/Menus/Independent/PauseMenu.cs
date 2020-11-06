using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : BaseInGameMenu
{
    [SerializeField] private Button _settingsButton = null;
    [SerializeField] private Button _exitToTitleScreenButton = null;
    [SerializeField] private GameObject _settingsMenu = null;
    private NodeManager _nodeManager = null;

    protected override void Awake()
    {
        base.Awake();
        _settingsButton.onClick.AddListener(() => SpawnMenu(_settingsMenu));
        _exitToTitleScreenButton.onClick.AddListener(ToMainMenu);
        _nodeManager = GameObject.FindObjectOfType<NodeManager>();
    }

    protected override void CloseMenu()
    {
        _nodeManager.Continue();
        base.CloseMenu();
    }
}
