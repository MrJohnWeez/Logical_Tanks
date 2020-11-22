using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeTypesMenu : BaseMenu
{
    [SerializeField] private Button[] _buttons = null;
    [SerializeField] private GameObject[] _menus = null;

    protected override void Awake()
    {
        base.Awake();
        for(int i = 0; i < _buttons.Length; i++)
        {
            int newI = i;
            _buttons[i].onClick.AddListener(() => SpawnMenu(_menus[newI]));
        }
    }
}
