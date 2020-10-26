using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotGate : LogicGateBase
{
    protected void Start()
    {
        StateSwitched(false);
    }

    protected override void StateSwitched(bool isOn)
    {
        EnergizeOutCabels(inCable1 && !inCable1.IsEnergized);
    }
}
