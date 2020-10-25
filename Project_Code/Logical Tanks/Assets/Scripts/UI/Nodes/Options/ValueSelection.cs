using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ValueSelection : MonoBehaviour
{
    [SerializeField] private Button _leftButton = null;
    [SerializeField] private Button _rightButton = null;
    [SerializeField] protected float minValue = -1;
    [SerializeField] protected float maxValue = 1;
    [SerializeField] protected float step = 1f;
    [SerializeField] protected float currentValue = 1;
    [SerializeField] protected bool shouldLoop = false;

    protected virtual void Awake()
    {
        _leftButton.onClick.AddListener(() => ValueChanged(false));
        _rightButton.onClick.AddListener(() => ValueChanged(true));
    }

    protected virtual void ValueChanged(bool increase)
    {
        currentValue += increase ? step : step * -1;
        if (shouldLoop)
        {
            if (currentValue > maxValue) { currentValue = minValue; }
            else if (currentValue < minValue) { currentValue = maxValue; }
        }
        else
        {
            currentValue = Mathf.Clamp(currentValue, minValue, maxValue);
        }
    }
}
