using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Action<int> OnTankValueChanged;
    [SerializeField] private GameObject[] _descriptions = null;
    [SerializeField] private Button _menuButton = null;
    [SerializeField] private Button _helpButton = null;
    [SerializeField] private GameObject _titleMenu = null;
    [SerializeField] private GameObject _pauseMenu = null;
    [SerializeField] private GameObject _helpMenu = null;
    [SerializeField] private GameObject _levelCompleteMenu = null;
    [SerializeField] private Canvas _inGameMenuCanvas = null;
    [SerializeField] private int _levelNumber = 0;
    [SerializeField] private int _numberOfTanksToWin = 1;
    [SerializeField] private TMP_Text _debugText = null;
    private int _currentNumberOfTanks = 0;
    private TankController[] _tankControllers = new TankController[7];
    private int _indirectMultiplier = 0;
    private NodeManager _nodeManager = null;
    private Action OnTankEnter;
    private Action OnTankExit;
    private float _prevTimeScale = 1;

    public int IndirectMultiplier => _indirectMultiplier;
    public GameObject[] Descriptions => _descriptions;
    public int LevelNumber => _levelNumber;
    public int NumberOfTanksToWin => _numberOfTanksToWin;

    private void Awake()
    {
        OnTankEnter = () => CompleteTankChanged(true);
        OnTankExit = () => CompleteTankChanged(false);
        _nodeManager = GameObject.FindObjectOfType<NodeManager>();
        GoalArea.OnTankEnter += OnTankEnter;
        GoalArea.OnTankExit += OnTankExit;
        _menuButton?.onClick.AddListener(OpenPauseMenu);
        _helpButton?.onClick.AddListener(OpenHelpMenu);
        QualitySettings.SetQualityLevel(SaveData.QualityLevel);
    }

    private void Start()
    {
        UpdateTanks();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SaveManager.PlayerSaveData.testInt++;
            SaveManager.Save();
            Debug.Log("Saved: " + SaveManager.PlayerSaveData.testInt);
        }
        //_debugText.text = "Test Int: " + SaveManager.PlayerSaveData.testInt.ToString();
        _debugText.text = "\nPath: " + SaveManager.GetSaveDataPath();
    }

    void OnDestroy()
    {
        GoalArea.OnTankEnter -= OnTankEnter;
        GoalArea.OnTankExit -= OnTankExit;
    }

    public void ResetGameSpeed() { _indirectMultiplier = 1; }
    public void Pause()
    {
        _indirectMultiplier = 0;
        _prevTimeScale = Time.timeScale;
        Time.timeScale = 1;
    }
    public void Continue()
    {
        _indirectMultiplier = 1;
        Time.timeScale = _prevTimeScale;
    }

    public void Stop()
    {
        Time.timeScale = 1;
        ResettableObject[] resetObjects = GameObject.FindObjectsOfType<ResettableObject>(true);
        _indirectMultiplier = 0;
        for (int i = resetObjects.Length - 1; i >= 0; i--) { resetObjects[i].ResetObject(); }
    }

    public void UpdateTanks()
    {
        TankController[] controllers = FindObjectsOfType<TankController>(true);
        foreach (TankController tc in controllers)
        {
            if (tc) { _tankControllers[(int)tc.GetColorID] = tc; }
        }
    }

    public TankController GetTankController(ColorID color) { return _tankControllers[(int)color]; }
    public void OpenPauseMenu() { SpawnMenu(_pauseMenu); }
    public void OpenHelpMenu() { SpawnMenu(_helpMenu); }
    public void OpenLevelCompleteMenu() { SpawnMenu(_levelCompleteMenu); }
    public void ToTitleScreen() { SceneManager.LoadScene("MainMenu"); }

    public void ToLevelSelection()
    {
        GameObject menu = SpawnMenu(_titleMenu);
        TitleScreenMenu titleScreenMenu = menu.GetComponent<TitleScreenMenu>();
        titleScreenMenu.ForceOpenLevelMenu();
    }

    private GameObject SpawnMenu(GameObject prefabToSpawn)
    {
        _nodeManager.Pause();
        GameObject menu = Instantiate(prefabToSpawn, _inGameMenuCanvas.gameObject.transform, false);
        return menu;
    }

    private void CompleteTankChanged(bool increased)
    {
        _currentNumberOfTanks += increased ? 1 : -1;
        OnTankValueChanged?.Invoke(_currentNumberOfTanks);
        if (_currentNumberOfTanks >= _numberOfTanksToWin) { OpenLevelCompleteMenu(); }
    }
}
