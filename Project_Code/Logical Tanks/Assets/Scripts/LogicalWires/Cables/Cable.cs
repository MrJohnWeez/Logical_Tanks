using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    public Action<bool> OnStateChanged;
    private Wire[] _wires = null;
    private bool _isEnergized = false;

    public bool IsEnergized => _isEnergized;

    private void Awake()
    {
        _wires = GetComponentsInChildren<Wire>();
    }

    public void SetEnergy(bool isEnergizedNow)
    {
        if(_isEnergized != isEnergizedNow)
        {
            _isEnergized = isEnergizedNow;
            foreach(Wire wire in _wires) { wire?.SetEnergy(_isEnergized); }
            OnStateChanged?.Invoke(_isEnergized);
        }
    }
}
