using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInGameMenu : BaseMenu
{
    protected GameManager gameManager = null;

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    protected virtual void ToMainMenu() { gameManager.ToTitleScreen(); }
}
