using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// TODO:
// Fix tank rotation and possible movement glitches
// Test on WebGL

// Start making actual levels

// List of features:
// - **** Move Tank Node
// - **** Rotate Tank Node
// - **** Rotate Turret Node
// - **** Shoot Tank Node
// - **** Loop Node
// - **** JumpTo Node
// - **** ReRout Node
// - Pressure Plate that triggers for all
// - **** Pressure plate that triggers for tank color
// - **** Black Door raises and lowers dependent on power received
// - **** Logic gates (And,Or,Not)
// - **** Capacitor (Charges for time it is powered then discharges)
// - **** Turret will shoot but does not rotate (variable cooldown)

public class GameManager : MonoBehaviour
{
    public Action<float> OnGameSpeedChanged;
    public Action<int> OnTankValueChanged;
    [SerializeField] private GameObject[] _descriptions = null;
    [SerializeField] private Button _menuButton = null;
    [SerializeField] private Button _helpButton = null;
    [SerializeField] private GameObject _pauseMenu = null;
    [SerializeField] private GameObject _helpMenu = null;
    [SerializeField] private GameObject _levelCompleteMenu = null;
    [SerializeField] private Canvas _inGameMenuCanvas = null;
    [SerializeField] private int _levelNumber = 0;
    [SerializeField] private int _numberOfTanksToWin = 1;
    private int _currentNumberOfTanks = 0;
    private float _gameSpeed = 0;
    private float _oldGameSpeed = 1.0f;
    private float _normalGameSpeed = 1.0f;
    private NodeManager _nodeManager = null;

    public float GameSpeed => _gameSpeed;
    public GameObject[] Descriptions => _descriptions;
    public int LevelNumber => _levelNumber;
    public int NumberOfTanksToWin => _numberOfTanksToWin;

    private void Awake()
    {
        _nodeManager = GameObject.FindObjectOfType<NodeManager>();
        GoalArea.OnTankEnter += () => CompleteTankChanged(true);
        GoalArea.OnTankExit += () => CompleteTankChanged(false);
        _menuButton?.onClick.AddListener(OpenSettingsMenu);
        _helpButton?.onClick.AddListener(OpenHelpMenu);
    }

    public void ResetGameSpeed() { SetGameSpeed(_normalGameSpeed); }

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
        SetGameSpeed(0);
        for (int i = resetObjects.Length - 1; i >= 0; i--) { resetObjects[i].ResetObject(); }
    }

    public void SetGameSpeed(float newSpeed)
    {
        _gameSpeed = newSpeed;
        OnGameSpeedChanged?.Invoke(_gameSpeed);
    }

    public void OpenSettingsMenu() { SpawnMenu(_pauseMenu); }
    public void OpenHelpMenu() { SpawnMenu(_helpMenu); }
    public void OpenLevelCompleteMenu() { SpawnMenu(_levelCompleteMenu); }
    public void ToTitleScreen() { SceneManager.LoadScene("MainMenu"); }

    private void SpawnMenu(GameObject prefabToSpawn)
    {
        _nodeManager.Pause();
        Instantiate(prefabToSpawn, _inGameMenuCanvas.gameObject.transform, false);
    }

    private void CompleteTankChanged(bool increased)
    {
        _currentNumberOfTanks += increased ? 1 : -1;
        OnTankValueChanged?.Invoke(_currentNumberOfTanks);
    }
}
