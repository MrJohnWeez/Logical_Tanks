using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatSelection : ValueSelection
{
    [SerializeField] private TMP_Text _label = null;
    [SerializeField] private string postFix = " m";

    protected override void ValueChanged(bool increase)
    {
        base.ValueChanged(increase);
        _label.text = currentValue.ToString() + postFix;
    }

    public float GetValue()
    {
        return currentValue;
    }
}
