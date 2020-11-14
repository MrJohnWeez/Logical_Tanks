using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TankCountDisplay : BaseInGameMenu
{
    [SerializeField] private TMP_Text _displayLabel = null;

    protected override void Awake()
    {
        base.Awake();
        UpdateTankLabel(0);
        gameManager.OnTankValueChanged += UpdateTankLabel;
    }

    private void OnDestroy() { gameManager.OnTankValueChanged -= UpdateTankLabel; }

    private void UpdateTankLabel(int newTankNumber)
    {
        string displayString = gameManager.NumberOfTanksToWin > 1 ? "Goal: {0}\\{1} Tanks" : "Goal: {0}\\{1} Tank";
        _displayLabel.text = string.Format(displayString, newTankNumber, gameManager.NumberOfTanksToWin);
    }
}
