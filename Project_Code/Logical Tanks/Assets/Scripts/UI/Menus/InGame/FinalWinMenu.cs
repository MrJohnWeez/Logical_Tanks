using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalWinMenu : BaseInGameMenu
{
    [SerializeField] private Button _claimRewardButton = null;

    protected override void Awake()
    {
        base.Awake();
        _claimRewardButton.onClick.AddListener(ClaimRewardNow);
    }

    private void ClaimRewardNow()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ&list=PLahKLy8pQdCM0SiXNn3EfGIXX19QGzUG3");
    }

    protected override void CloseMenu() { gameManager.ToLevelSelection(); }
}
