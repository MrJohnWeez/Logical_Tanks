using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Action<float> OnGameSpeedChanged;
    [SerializeField] private GameObject[] _descriptions = null;
    [SerializeField] private Button _menuButton = null;
    [SerializeField] private Button _helpButton = null;
    [SerializeField] private GameObject _pauseMenu = null;
    [SerializeField] private GameObject _helpMenu = null;
    [SerializeField] private GameObject _levelCompleteMenu = null;
    [SerializeField] private Canvas _inGameMenuCanvas = null;
    [SerializeField] private int _levelNumber = 0;
    [SerializeField] private int _numberOfTanksToWin = 1;
    private float _gameSpeed = 0;
    private float _oldGameSpeed = 1.0f;
    private float _normalGameSpeed = 1.0f;
    private NodeManager _nodeManager = null;

    public float GameSpeed => _gameSpeed;
    public GameObject[] Descriptions => _descriptions;
    public int LevelNumber => _levelNumber;

    private void Awake()
    {
        _nodeManager = GameObject.FindObjectOfType<NodeManager>();
        GoalArea.OnTankNumberCompleted += OpenLevelCompleteMenu;
        _menuButton?.onClick.AddListener(OpenSettingsMenu);
        _helpButton?.onClick.AddListener(OpenHelpMenu);
        GoalArea.NumberOfTanksToWin =  _numberOfTanksToWin;
    }

    public void ResetGameSpeed()
    {
        SetGameSpeed(_normalGameSpeed);
    }

    public void Pause()
    {
        _oldGameSpeed = _gameSpeed;
        SetGameSpeed(0);
    }

    public void Continue()
    {
        _gameSpeed = _oldGameSpeed;
        SetGameSpeed(_gameSpeed);
    }

    public void Stop()
    {
        ResettableObject[] resetObjects = GameObject.FindObjectsOfType<ResettableObject>(true);
        for (int i = resetObjects.Length - 1; i >= 0; i--) { resetObjects[i].ResetObject(); }
        SetGameSpeed(0);
    }

    public void SetGameSpeed(float newSpeed)
    {
        _gameSpeed = newSpeed;
        OnGameSpeedChanged?.Invoke(_gameSpeed);
    }

    public void OpenSettingsMenu()
    {
        _nodeManager.Pause();
        SpawnMenu(_pauseMenu);
    }

    public void OpenHelpMenu()
    {
        _nodeManager.Pause();
        SpawnMenu(_helpMenu);
    }

    public void OpenLevelCompleteMenu()
    {
        _nodeManager.Pause();
        SpawnMenu(_levelCompleteMenu);
    }

    public void ToTitleScreen() { SceneManager.LoadScene("MainMenu"); }

    private void SpawnMenu(GameObject prefabToSpawn)
    {
        Instantiate(prefabToSpawn, _inGameMenuCanvas.gameObject.transform, false);
    }
}
