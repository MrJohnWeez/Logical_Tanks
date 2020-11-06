using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelDescription : BaseInGameMenu
{
    private GameObject[] _descriptions = null;
    private Button _rightArrow = null;
    private Button _leftArrow = null;
    private int _currentDescNumber = 0;
    private GameObject _currentDesc = null;
    
    protected override void Awake()
    {
        base.Awake();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        _rightArrow = GameObject.FindGameObjectWithTag("RightDescArrow").GetComponent<Button>();
        _leftArrow = GameObject.FindGameObjectWithTag("LeftDescArrow").GetComponent<Button>();
        _rightArrow.onClick.AddListener(() => NextDesc(true));
        _leftArrow.onClick.AddListener(() => NextDesc(false));
    }

    private void Start()
    {
        _descriptions = gameManager.Descriptions;
        if(_descriptions.Length == 1)
        {
            _leftArrow.gameObject.SetActive(false);
            _rightArrow.gameObject.SetActive(false);
        }
        if(_descriptions.Length > 0) { UpdateDesc(); }
    }

    private void NextDesc(bool shouldIncrease)
    {
        if(_descriptions.Length > 0)
        {
            int prevNumber = _currentDescNumber;
            _currentDescNumber += shouldIncrease ? 1 : -1;
            _currentDescNumber = Mathf.Clamp(_currentDescNumber, 0, _descriptions.Length);
            if(prevNumber != _currentDescNumber) { UpdateDesc(); }
        }
    }

    private void UpdateDesc()
    {
        if(_currentDesc) { Destroy(_currentDesc); }
        _currentDesc = Instantiate(_descriptions[_currentDescNumber], transform);
        _leftArrow.interactable = _currentDescNumber > 0;
        _rightArrow.interactable = _currentDescNumber < _descriptions.Length - 1;
    }
}
