using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTankNode : TankNode
{
    public override void Execute()
    {
        base.Execute();
        float distance = floatSelection.GetValue();
        _tankController?.MoveTank(distance);
    }
}
