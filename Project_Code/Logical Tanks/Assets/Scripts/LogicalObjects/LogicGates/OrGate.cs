using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrGate : LogicGateBase
{
    protected override void StateSwitched(bool isOn)
    {
        EnergizeOutCable(inCable1 && inCable2 && inCable1.IsEnergized || inCable2.IsEnergized);
    }
}
