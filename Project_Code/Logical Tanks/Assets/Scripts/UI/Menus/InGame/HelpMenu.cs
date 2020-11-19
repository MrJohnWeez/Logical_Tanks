using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpMenu : BaseMenu
{
    [SerializeField] private Button _nodeTypes = null;
    [SerializeField] private Button _nodeSelection = null;
    [SerializeField] private Button _playControls = null;
    [SerializeField] private Button _errors = null;
    [SerializeField] private GameObject _nodeTypesMenuPrefab = null;
    [SerializeField] private GameObject _nodeSelectionMenuPrefab = null;
    [SerializeField] private GameObject _playControlsMenuPrefab = null;
    [SerializeField] private GameObject _errorsMenuPrefab = null;

    protected override void Awake()
    {
        base.Awake();
        _nodeTypes.onClick.AddListener(() => SpawnMenu(_nodeTypesMenuPrefab));
        _nodeSelection.onClick.AddListener(() => SpawnMenu(_nodeSelectionMenuPrefab));
        _playControls.onClick.AddListener(() => SpawnMenu(_playControlsMenuPrefab));
        _errors.onClick.AddListener(() => SpawnMenu(_errorsMenuPrefab));
    }
}
