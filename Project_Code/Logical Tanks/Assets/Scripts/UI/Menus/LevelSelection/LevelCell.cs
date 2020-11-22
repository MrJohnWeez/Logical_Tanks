using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelCell : MonoBehaviour
{
    private const string SCENE_PREFIX = "Level ";
    [SerializeField] private GameObject _lockImage = null;
    [SerializeField] private GameObject _checkmarkImage = null;
    [SerializeField] private GameObject _lockedPanel = null;
    [SerializeField] private TMP_Text _titleTMP = null;
    [SerializeField] private TMP_Text _levelNumberTMP = null;
    [SerializeField] private Image _levelPreview = null;
    [SerializeField] private Button _button = null;
    private bool _isLocked = true;
    private bool _isCompleted = false;
    private int _levelNumber = 0;
    

    public void PopulateCell(int levelNumber, string levelTitle, Sprite levelPreview, bool IsLocked, bool IsCompleted)
    {
        _levelPreview.sprite = levelPreview;
        _levelNumberTMP.text = levelNumber.ToString();
        _levelNumber = levelNumber;
        _titleTMP.text = levelTitle;
        _isLocked = IsLocked;
        _isCompleted = IsCompleted;
        UpdateSelf();
    }

    private void Awake()
    {
        _button.onClick.AddListener(GoToScene);
    }
    
    private void UpdateSelf()
    {
        _lockImage.SetActive(_isLocked);
        _checkmarkImage.SetActive(_isCompleted);
        _lockedPanel.SetActive(_isLocked);
    }

    private void GoToScene()
    {
        if(!_isLocked)
        {
            SceneManager.LoadSceneAsync(SCENE_PREFIX + _levelNumber.ToString());
        }
        else
        {
            // TODO: Play error sound or something
        }
    }
}
