using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGateBase : VariableCycledObject
{
    [SerializeField] protected LogicCable inCable1 = null;
    [SerializeField] protected LogicCable inCable2 = null;
    [SerializeField] protected LogicCable outCable = null;

    protected override void Awake()
    {
        base.Awake();
        if(inCable1) { inCable1.OnStateChanged += StateSwitched; }
        if(inCable2) { inCable2.OnStateChanged += StateSwitched; }
    }

    public override void ResetObject() { }
    protected virtual void StateSwitched(bool isOn) { }
    protected virtual void EnergizeOutCable(bool energize) { outCable?.SetEnergy(energize); }
}
