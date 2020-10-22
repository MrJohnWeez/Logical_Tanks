using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(UILineRenderer))]
public class ColoredLine : ColorChanger
{
    [Header("ColoredLine")]
    protected UILineRenderer _uILineRenderer = null;

    public virtual void Awake()
    {
        _uILineRenderer = GetComponent<UILineRenderer>();
        _startColor = _uILineRenderer.color;
    }

    protected override IEnumerator ChangeColor(Color newColor, float fadeTime = 0)
    {
        if (waitingTask != null)
        {
            currentTask?.Stop();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            currentTask = waitingTask;
            waitingTask = null;
        }
        float currentTime = 0;
        Color startColor = _uILineRenderer.color;
        while (currentTime < fadeTime && fadeTime > 0)
        {
            currentTime += Time.deltaTime;
            _uILineRenderer.color = Color.Lerp(startColor, newColor, currentTime / fadeTime);
            yield return new WaitForFixedUpdate();
        }
        _uILineRenderer.color = newColor;
        currentTask = null;
    }

    public override void SetThenResetColor(Color newColor, float fadeTime = 0)
    {
        _uILineRenderer.color = newColor;
        base.SetThenResetColor(newColor, fadeTime);
    }
}
