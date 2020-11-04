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
    private float _startCharge = 0.0f;
    
    protected override void StateSwitched(bool isOn) { _isCharging = inCable1 && inCable1.IsEnergized; }

    protected override void Start()
    {
        base.Start();
        _startCharge = _currentCharge; 
    }

    void Update()
    {
        if(_isCharging && _currentCharge < MAX_CHARGE)
        {
            _currentCharge += Time.deltaTime * gameManager.GameSpeed;
            _currentCharge = Mathf.Clamp(_currentCharge, MIN_CHARGE, MAX_CHARGE);
            UpdateText();
        }
        else if(!_isCharging && _currentCharge > MIN_CHARGE)
        {
            _currentCharge -= Time.deltaTime * gameManager.GameSpeed;
            _currentCharge = Mathf.Clamp(_currentCharge, MIN_CHARGE, MAX_CHARGE);
            UpdateText();
        }
        
        if(_currentCharge > MIN_CHARGE && _prevCurrentCharge == MIN_CHARGE) { EnergizeOutCabels(true); }
        else if(_currentCharge == MIN_CHARGE && _prevCurrentCharge > MIN_CHARGE) { EnergizeOutCabels(false); }
        _prevCurrentCharge = _currentCharge;
    }

    public override void ResetObject()
    {
        _currentCharge = _startCharge;
        UpdateText();
        base.ResetObject();
    }

    private void UpdateText()
    {
        _chargeTitle.text = _currentCharge.ToString("F1");
    }
}
