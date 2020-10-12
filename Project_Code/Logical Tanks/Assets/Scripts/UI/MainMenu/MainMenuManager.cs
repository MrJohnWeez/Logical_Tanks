using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject _settingsMenu = null;
    [SerializeField] private GameObject _aboutMenu = null;
    [SerializeField] private GameObject _levelSelectionMenu = null;

    [Header("Buttons")]
    [SerializeField] private Button[] _closeMenuButtons = null;
    [SerializeField] private Button _openSettings = null;
    [SerializeField] private Button _openAbout = null;
    [SerializeField] private Button _openLevelSelection = null;

    private void Awake()
    {
        foreach(Button button in _closeMenuButtons) { button.onClick.AddListener(CloseAll); }
        _openSettings.onClick.AddListener(OpenSettings);
        _openAbout.onClick.AddListener(OpenAbout);
        _openLevelSelection.onClick.AddListener(OpenLevelSelection);
    }

    private void Start()
    {
        CloseAll();
    }

    private void CloseAll()
    {
        _settingsMenu.SetActive(false);
        _aboutMenu.SetActive(false);
        _levelSelectionMenu.SetActive(false);
    }

    private void OpenSettings()
    {
        _settingsMenu.SetActive(true);
    }

    private void OpenAbout()
    {
        _aboutMenu.SetActive(true);
    }

    private void OpenLevelSelection()
    {
        _levelSelectionMenu.SetActive(true);
    }
}
