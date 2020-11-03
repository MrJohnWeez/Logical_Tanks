using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ColorManager : VariableCycledObject
{
    [Header("ColorChanger")]
    [HideInInspector] [NonSerialized] public float iterationFadeTime = 1.0f;
    [HideInInspector] [NonSerialized] public Color highlightColor = Color.blue;
    [HideInInspector] [NonSerialized] public Color iterationColor = Color.yellow;
    protected Color _startColor;
    protected float currentFadeTime = 0;
    protected float fadeTime = 0;
    protected Color oldColor;
    protected Color newColor;
    protected Color targetColor;

    protected virtual void Update()
    {
        if (currentFadeTime < fadeTime)
        {
            currentFadeTime += Time.deltaTime * gameManager.GameSpeed;
            if (currentFadeTime > fadeTime) { currentFadeTime = fadeTime; }
            newColor = Color.Lerp(oldColor, targetColor, currentFadeTime / fadeTime);
            UpdateColor();
        }
    }

    protected virtual void ChangeColor(Color newColor, float newFadeTime = 0)
    {
        targetColor = newColor;
        currentFadeTime = 0;
        fadeTime = newFadeTime;
        if (fadeTime == 0)
        {
            this.newColor = newColor;
            UpdateColor();
        }
    }

    protected virtual void UpdateColor() { }
    public override void ResetObject()
    {
        currentFadeTime = 0;
        fadeTime = 0;
        newColor = _startColor;
        UpdateColor();
    }

    public virtual void ResetColor(float fadeTime = 0) { ChangeColor(_startColor, fadeTime); }
    public virtual void SetThenResetColor(Color newColor, float fadeTime = 0) { ResetColor(fadeTime); }
    public virtual Color GetStartColor() { return _startColor; }
    public virtual void SetStartColor(Color newColor) { _startColor = newColor; }
}
