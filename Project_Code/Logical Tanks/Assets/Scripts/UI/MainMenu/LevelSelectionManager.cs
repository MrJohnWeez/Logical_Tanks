using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    [SerializeField] private GameObject _cellPrefab = null;
    [SerializeField] private Sprite[] _levelPreviews = null;
    [SerializeField] private string[] _levelTitles = null;

    private void Awake()
    {
        if(_levelPreviews.Length != _levelTitles.Length)
        {
            Debug.LogError("Invalid Level Configurations!");
        }
    }
    private void Start()
    {
        for(int levelNum = 0; levelNum < _levelTitles.Length; levelNum++)
        {
            GameObject newCell = Instantiate(_cellPrefab, transform);
            LevelCell levelCell = newCell.GetComponent<LevelCell>();
            levelCell.PopulateCell(levelNum + 1, _levelTitles[levelNum], _levelPreviews[levelNum], false, false);
        }
    }
}
