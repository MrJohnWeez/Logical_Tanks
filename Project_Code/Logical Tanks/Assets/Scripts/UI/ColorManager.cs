using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Image))]
public class ColorChanger : MonoBehaviour
{
    [HideInInspector] [NonSerialized] public float iterationFadeTime = 1.0f;
    [HideInInspector] [NonSerialized] public Color highlightColor = Color.blue;
    [HideInInspector] [NonSerialized] public Color iterationColor = Color.yellow;
    protected Color _startColor;
    protected Image _image = null;
    protected Task currentTask = null;
    protected Task waitingTask = null;

    public virtual void Awake() { _image = GetComponent<Image>(); }
    public virtual void Start()
    {
        if (_image)
            _startColor = _image.color;
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

        float currentTime = 0;
        Color startColor = _image.color;
        while (currentTime < fadeTime && fadeTime > 0)
        {
            currentTime += Time.deltaTime;
            _image.color = Color.Lerp(startColor, newColor, currentTime / fadeTime);
            yield return new WaitForFixedUpdate();
        }
        _image.color = newColor;
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
        _image.color = newColor;
        ResetColor(fadeTime);
    }
}
