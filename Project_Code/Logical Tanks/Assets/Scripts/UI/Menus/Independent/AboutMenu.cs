using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboutMenu : BaseMenu
{
    private enum ResetType
    {
        NONE,
        R,
        Re,
        Res,
        Rese,
        Reset,
        Reset_,
        Reset_G,
        Reset_Ga,
        Reset_Gam,
        Reset_Game
    }
    
    [SerializeField] private GameObject _unlockedLevelsPrefab = null;
    [SerializeField] private GameObject _resetGamePrefab = null;
    [SerializeField] private Button _secretButton = null;
    private int _counter = 0;
    private ResetType _resetType = ResetType.NONE;

    protected override void Awake()
    {
        base.Awake();
        _secretButton.onClick.AddListener(UnlockAllLevels);
    }

    private void Update()
    {
        Debug.Log(_resetType);
        // TODO: This is just a quick way to implementing this
        switch(_resetType)
        {
            case ResetType.NONE:
                if(Input.GetKeyDown(KeyCode.R)) { _resetType = ResetType.R; }
                break;
            case ResetType.R:
                if(Input.GetKeyDown(KeyCode.E)) { _resetType = ResetType.Re; }
                break;
            case ResetType.Re:
                if(Input.GetKeyDown(KeyCode.S)) { _resetType = ResetType.Res; }
                break;
            case ResetType.Res:
                if(Input.GetKeyDown(KeyCode.E)) { _resetType = ResetType.Rese; }
                break;
            case ResetType.Rese:
                if(Input.GetKeyDown(KeyCode.T)) { _resetType = ResetType.Reset; }
                break;
            case ResetType.Reset:
                if(Input.GetKeyDown(KeyCode.Space)) { _resetType = ResetType.Reset_; }
                break;
            case ResetType.Reset_:
                if(Input.GetKeyDown(KeyCode.G)) { _resetType = ResetType.Reset_G; }
                break;
            case ResetType.Reset_G:
                if(Input.GetKeyDown(KeyCode.A)) { _resetType = ResetType.Reset_Ga; }
                break;
            case ResetType.Reset_Ga:
                if(Input.GetKeyDown(KeyCode.M)) { _resetType = ResetType.Reset_Gam; }
                break;
            case ResetType.Reset_Gam:
                if(Input.GetKeyDown(KeyCode.E)) { _resetType = ResetType.Reset_Game; }
                break;
            case ResetType.Reset_Game:
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    SaveData.ResetGameData();
                    GameObject notification = Instantiate(_resetGamePrefab, transform);
                    Destroy(notification, 5f);
                }
                break;
        }
    }

    private void UnlockAllLevels()
    {
        _counter++;
        if(_counter == 10)
        {
            SaveData.UnlockAllLevels();
            GameObject notification = Instantiate(_unlockedLevelsPrefab, transform);
            Destroy(notification, 5f);
        }
    }
}
