using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LogicCable : MonoBehaviour
{
    public Action<bool> OnStateChanged;
    [SerializeField] private LineRenderer _lineRenderer = null;
    [SerializeField] private Color _energyColor = Color.yellow;
    private bool _isEnergized = false;
    private Color _startColor;

    public bool IsEnergized => _isEnergized;  
    
    public void SetEnergy(bool isEnergizedNow)
    {
        if(_isEnergized != isEnergizedNow)
        {
            _isEnergized = isEnergizedNow;
            SetWireColor(isEnergizedNow ? _energyColor : _startColor);
            OnStateChanged?.Invoke(_isEnergized);
        }
    }

    private void Awake() { _startColor = _lineRenderer.startColor; } 
    private void SetWireColor(Color newColor) { _lineRenderer.startColor = _lineRenderer.endColor = newColor; }
}
