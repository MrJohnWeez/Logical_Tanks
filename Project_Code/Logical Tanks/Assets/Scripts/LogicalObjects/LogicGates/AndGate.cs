using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndGate : LogicGateBase
{
    protected override void StateSwitched(bool isOn)
    {
        EnergizeOutCabels(inCable1 && inCable2 && inCable1.IsEnergized && inCable2.IsEnergized);
    }
}
