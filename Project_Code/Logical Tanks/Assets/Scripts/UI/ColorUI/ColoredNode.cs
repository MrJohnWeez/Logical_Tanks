using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColoredImage : ColorChanger
{
    [Header("ColoredImage")]
    [SerializeField] protected Image image = null;

    public virtual void Awake()
    {
        _startColor = image.color;
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
        Color startColor = image.color;
        while (currentTime < fadeTime && fadeTime > 0)
        {
            currentTime += Time.deltaTime;
            image.color = Color.Lerp(startColor, newColor, currentTime / fadeTime);
            yield return new WaitForFixedUpdate();
        }
        image.color = newColor;
        currentTask = null;
    }

    public override void SetThenResetColor(Color newColor, float fadeTime = 0)
    {
        image.color = newColor;
        base.SetThenResetColor(newColor, fadeTime);
    }
}
