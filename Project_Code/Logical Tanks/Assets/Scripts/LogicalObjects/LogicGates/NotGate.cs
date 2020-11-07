using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotGate : LogicGateBase
{
    protected override void Start()
    {
        base.Start();
        StateSwitched(false);
    }

    protected override void StateSwitched(bool isOn)
    {
        EnergizeOutCable(inCable1 && !inCable1.IsEnergized);
    }
}
