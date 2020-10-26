using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicGateBase : MonoBehaviour
{
    [SerializeField] protected Cable inCable1 = null;
    [SerializeField] protected Cable inCable2 = null;
    [SerializeField] protected Cable[] outCabels = null;

    protected virtual void Awake()
    {
        if(inCable1) { inCable1.OnStateChanged += StateSwitched; }
        if(inCable2) { inCable2.OnStateChanged += StateSwitched; }
    }

    protected virtual void StateSwitched(bool isOn) { }

    protected virtual void EnergizeOutCabels(bool energize)
    {
        foreach(Cable cable in outCabels) { cable.SetEnergy(energize); }
    }
}
