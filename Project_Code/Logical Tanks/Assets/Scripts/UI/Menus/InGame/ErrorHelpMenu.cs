using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorHelpMenu : BaseMenu
{
    [SerializeField] private Button _infMenuButton = null;
    [SerializeField] private Button _tankCollisionMenuButton = null;
    [SerializeField] private GameObject _infErrorMenuPrefab = null;
    [SerializeField] private GameObject _tankCollisionErrorMenuPrefab = null;

    protected override void Awake()
    {
        base.Awake();
        _infMenuButton?.onClick.AddListener(() => SpawnMenu(_infErrorMenuPrefab));
        _tankCollisionMenuButton?.onClick.AddListener(() => SpawnMenu(_tankCollisionErrorMenuPrefab));
    }
}
