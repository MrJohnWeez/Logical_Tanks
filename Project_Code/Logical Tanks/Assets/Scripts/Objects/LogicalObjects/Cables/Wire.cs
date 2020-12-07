using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    [SerializeField] private GameObject _wireModel = null;
    [SerializeField] private Material _energyColor = null;
    private Material _material;
    private Color _startColor;

    private void Awake()
    {
        _material = _wireModel.GetComponent<Renderer>().material;
        _startColor = _material.color;
    }

    public void SetEnergy(bool isEnergizedNow)
    {
        _material.color = isEnergizedNow ? _energyColor.color : _startColor;
    }
}
