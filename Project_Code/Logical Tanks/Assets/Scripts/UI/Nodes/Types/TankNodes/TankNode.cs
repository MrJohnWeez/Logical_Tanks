﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankNode : Node
{
    [SerializeField] protected ColorSelection colorSelection = null;
    [SerializeField] protected FloatSelection floatSelection = null;
    protected TankController _tankController = null;

    public override void Execute()
    {
        RunNodeColor(true);
        _tankController = gameManager.GetTankController(colorSelection.GetColorID());
        if(_tankController && _tankController.gameObject.activeSelf) { _tankController.OnTankStateChangedToIdle += OnExecuteFinished; }
        else { OnExecuteFinished(); }
    }

    public override void OnExecuteFinished()
    {
        if(_tankController) { _tankController.OnTankStateChangedToIdle -= OnExecuteFinished; }
        base.OnExecuteFinished();
    }
}
