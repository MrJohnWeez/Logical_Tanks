using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PressurePlate : MonoBehaviour
{    
    [SerializeField] private GameObject _coloredObject = null;
    [SerializeField] private Cable[] _cables = null;
    private ColorID _colorID = ColorID.Green;
    private BoxCollider _trigger = null;

    public ColorID GetColorID => _colorID;

    private void Awake()
    {
        _trigger = GetComponent<BoxCollider>();
        _colorID = _coloredObject.GetComponent<Renderer>().material.GetMatchingColor();
    }

    private void OnTriggerEnter(Collider other) { Activate(other); }
    private void OnTriggerExit(Collider other) { Deactivate(other); }

    private void Activate(Collider other)
    {
        TankController tankController = other.gameObject.GetComponent<TankController>();
        if(tankController && tankController.GetColorID == _colorID)
        {
            foreach(Cable cable in _cables) { cable?.SetEnergy(true); }
        }
    }

    private void Deactivate(Collider other)
    {
        TankController tankController = other.gameObject.GetComponent<TankController>();
        if(tankController && tankController.GetColorID == _colorID)
        {
            foreach(Cable cable in _cables) { cable?.SetEnergy(false); }
        }
    }
}
