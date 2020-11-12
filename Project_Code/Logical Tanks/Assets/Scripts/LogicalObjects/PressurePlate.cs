using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PressurePlate : LogicGateBase
{    
    [SerializeField] private GameObject _coloredObject = null;
    private ColorID _colorID = ColorID.Green;
    private BoxCollider _trigger = null;
    private TankController _currentTank = null;

    public ColorID GetColorID => _colorID;

    protected override void Awake()
    {
        base.Awake();
        _trigger = GetComponent<BoxCollider>();
        _colorID = _coloredObject.GetComponent<Renderer>().material.GetMatchingColor();
    }

    private void OnTriggerEnter(Collider other) { SetEnergy(other, true); }
    private void OnTriggerExit(Collider other) { SetEnergy(other, false); }
    private void Update()
    {
        if(_currentTank && !_currentTank.IsReady)
        {
            _currentTank = null;
            EnergizeOutCable(false);
        }
    }

    private void SetEnergy(Collider other, bool hasEnergy)
    {
        TankController tankController = other.gameObject.GetComponent<TankController>();
        if(tankController && tankController.GetColorID == _colorID || (tankController && _colorID == ColorID.None))
        {
            _currentTank = tankController;
            EnergizeOutCable(hasEnergy);
        }
    }
}
