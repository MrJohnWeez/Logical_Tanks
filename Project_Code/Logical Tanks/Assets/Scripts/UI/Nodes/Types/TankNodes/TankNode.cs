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
        UpdateTankController();
        if(_tankController)
            _tankController.OnTankStateChangedToIdle += OnExecuteFinished;
    }

    public override void OnExecuteFinished()
    {
        if(_tankController) { _tankController.OnTankStateChangedToIdle -= OnExecuteFinished; }
        base.OnExecuteFinished();
    }

    protected virtual void UpdateTankController()
    {
        TankController[] controllers = FindObjectsOfType<TankController>();
        ColorID tankColor = colorSelection.GetColorID();
        foreach(TankController tc in controllers)
        {
            if(tc.GetColorID == tankColor)
            {
                _tankController = tc;
                break;
            }
        }
    }
}