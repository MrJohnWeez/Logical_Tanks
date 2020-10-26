using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Capacitor : LogicGateBase
{
    private const float MAX_CHARGE = 99.9f;
    private const float MIN_CHARGE = 0f;
    [SerializeField] private float _currentCharge = 0.0f;
    [SerializeField] private TMP_Text _chargeTitle = null;
    private bool _isCharging = false;
    private float _prevCurrentCharge = MIN_CHARGE;
    
    protected override void StateSwitched(bool isOn) { _isCharging = inCable1 && inCable1.IsEnergized; }

    void Update()
    {
        if(_isCharging && _currentCharge < MAX_CHARGE)
        {
            _currentCharge += Time.deltaTime;
            _currentCharge = Mathf.Clamp(_currentCharge, MIN_CHARGE, MAX_CHARGE);
            _chargeTitle.text = _currentCharge.ToString("F1");
        }
        else if(!_isCharging && _currentCharge > MIN_CHARGE)
        {
            _currentCharge -= Time.deltaTime;
            _currentCharge = Mathf.Clamp(_currentCharge, MIN_CHARGE, MAX_CHARGE);
            _chargeTitle.text = _currentCharge.ToString("F1");
        }
        
        if(_currentCharge > MIN_CHARGE && _prevCurrentCharge == MIN_CHARGE) { EnergizeOutCabels(true); }
        else if(_currentCharge == MIN_CHARGE && _prevCurrentCharge > MIN_CHARGE) { EnergizeOutCabels(false); }
        _prevCurrentCharge = _currentCharge;
    }
}
