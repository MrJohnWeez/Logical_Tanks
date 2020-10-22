using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ColorChanger : MonoBehaviour
{
    [Header("ColorChanger")]
    [HideInInspector] [NonSerialized] public float iterationFadeTime = 1.0f;
    [HideInInspector] [NonSerialized] public Color highlightColor = Color.blue;
    [HideInInspector] [NonSerialized] public Color iterationColor = Color.yellow;
    protected Color _startColor;
    protected Task currentTask = null;
    protected Task waitingTask = null;

    public virtual void OnDestroy()
    {
        currentTask?.Stop();
        waitingTask = null;
        currentTask = null;
    }

    protected virtual IEnumerator ChangeColor(Color newColor, float fadeTime = 0)
    {
        if (waitingTask != null)
        {
            currentTask?.Stop();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            currentTask = waitingTask;
            waitingTask = null;
        }
        currentTask = null;
    }

    public virtual void SetColor(Color newColor, float fadeTime = 0)
    {
        waitingTask = new Task(ChangeColor(newColor, fadeTime));
    }

    public virtual void ResetColor(float fadeTime = 0)
    {
        waitingTask = new Task(ChangeColor(_startColor, fadeTime));
    }

    public virtual void SetThenResetColor(Color newColor, float fadeTime = 0)
    {
        ResetColor(fadeTime);
    }
}
