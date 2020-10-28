﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColoredImage : ColorManager
{
    [Header("ColoredImage")]
    [SerializeField] protected Image image = null;

    protected virtual void Awake() { _startColor = image.color; }

    public override void Stop()
    {
        image.color = _startColor;
        base.Stop();
    }

    protected override void UpdateColor() { image.color = newColor; }

    public override void ResetColor(float fadeTime = 0)
    {
        oldColor = image.color;
        base.ResetColor(fadeTime);
    }

    public override void SetThenResetColor(Color newColor, float fadeTime = 0)
    {
        image.color = newColor;
        base.SetThenResetColor(newColor, fadeTime);
    }
}
