using System.Collections;
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
        if(_tankController && _tankController.gameObject.activeSelf) { _tankController.OnTankFinished += OnExecuteFinished; }
        else { OnExecuteFinished(); }
    }

    public override void OnExecuteFinished(int errorCode = 0)
    {
        if(_tankController) { _tankController.OnTankFinished -= OnExecuteFinished; }
        if(errorCode > 0)
        {
            if(errorCode == 1) { ChangeColor(Color.red); }
        }
        else
        {
            RunNodeColor(false);
        }
        OnFinishedExecution?.Invoke(this);
    }
}
