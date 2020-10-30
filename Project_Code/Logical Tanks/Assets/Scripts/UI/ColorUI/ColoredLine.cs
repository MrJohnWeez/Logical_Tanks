using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(UILineRenderer))]
public class ColoredLine : ColorManager
{
    [Header("ColoredLine")]
    protected UILineRenderer _uILineRenderer = null;

    protected override void Awake()
    {
        base.Awake();
        _uILineRenderer = GetComponent<UILineRenderer>();
        _startColor = _uILineRenderer.color;
    }

    public override void ResetObject()
    {
        _uILineRenderer.color = _startColor;
        base.ResetObject();
    }

    protected override void UpdateColor() { _uILineRenderer.color = newColor; }

    public override void ResetColor(float fadeTime = 0)
    {
        oldColor = _uILineRenderer.color;
        base.ResetColor(fadeTime);
    }

    public override void SetThenResetColor(Color newColor, float fadeTime = 0)
    {
        _uILineRenderer.color = newColor;
        base.SetThenResetColor(newColor, fadeTime);
    }
}
