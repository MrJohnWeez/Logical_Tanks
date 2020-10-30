using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTankTurretNode : TankNode
{    
    public override void Execute()
    {
        base.Execute();
        float degrees = floatSelection.GetValue();
        _tankController?.RotateTurret(degrees);
    }
}
