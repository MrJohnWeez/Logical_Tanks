using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PressurePlate : MonoBehaviour
{
    public Action OnActivate;
    public Action OnDeactivate;
    
    [SerializeField] private GameObject _coloredObject = null;
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
            Debug.Log("Activated pressure plate with color: " + _colorID);
            OnActivate?.Invoke();
        }
    }

    private void Deactivate(Collider other)
    {
        TankController tankController = other.gameObject.GetComponent<TankController>();
        if(tankController && tankController.GetColorID == _colorID)
        {
            Debug.Log("Deactivated pressure plate with color: " + _colorID);
            OnDeactivate?.Invoke();
        }
    }
}
