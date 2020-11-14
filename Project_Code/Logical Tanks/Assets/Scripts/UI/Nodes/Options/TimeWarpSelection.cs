using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeWarpSelection : ValueSelection
{
    [SerializeField] private TMP_Text _label = null;
    [SerializeField] private string postFix = " x";
    [SerializeField] private float[] _stepValues = null;

    protected override void Awake()
    {
        base.Awake();
        _label.text = GetValue().ToString() + postFix;
    }

    protected override void ValueChanged(bool increase)
    {
        base.ValueChanged(increase);
        _label.text = GetValue().ToString() + postFix;
    }

    public float GetValue()
    {
        return _stepValues[(int)currentValue];
    }
}
