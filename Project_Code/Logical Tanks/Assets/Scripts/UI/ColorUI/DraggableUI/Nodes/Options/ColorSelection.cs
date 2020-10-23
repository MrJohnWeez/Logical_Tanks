using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorSelection : ValueSelection
{
    [SerializeField] private Material[] _colorList = null;
    [SerializeField] private Image _image = null;
    private ColorID _colorID = ColorID.Green;

    protected override void ValueChanged(bool increase)
    {
        base.ValueChanged(increase);
        _colorID = (ColorID)(int)currentValue;
        _image.color = _colorList[(int)currentValue].color;
    }

    public ColorID GetColorID()
    {
        return _colorID;
    }
}
