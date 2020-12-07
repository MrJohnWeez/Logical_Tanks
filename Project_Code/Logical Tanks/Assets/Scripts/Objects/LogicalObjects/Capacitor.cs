using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Capacitor : LogicGateBase
{
    private const float MIN_CHARGE = 0f;
    [SerializeField] private float _maxCharge = 99.9f;
    [SerializeField] private float _currentCharge = 0.0f;
    [SerializeField] private TMP_Text _chargeTitle = null;
    private bool _isCharging = false;
    private float _prevCurrentCharge = -1;
    private float _startCharge = 0.0f;
    
    protected override void StateSwitched(bool isOn) { _isCharging = inCable1 && inCable1.IsEnergized; }

    protected override void Start()
    {
        base.Start();
        _startCharge = _currentCharge; 
        UpdateOutCable(true);
    }

    void Update()
    {
        if(_isCharging && _currentCharge < _maxCharge)
        {
            _currentCharge += Time.deltaTime * gameManager.IndirectMultiplier;
            _currentCharge = Mathf.Clamp(_currentCharge, MIN_CHARGE, _maxCharge);
            UpdateText();
        }
        else if(!_isCharging && _currentCharge > MIN_CHARGE)
        {
            _currentCharge -= Time.deltaTime * gameManager.IndirectMultiplier;
            _currentCharge = Mathf.Clamp(_currentCharge, MIN_CHARGE, _maxCharge);
            UpdateText();
        }
        if(_prevCurrentCharge != _currentCharge) { UpdateOutCable(); }
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

    private void UpdateOutCable(bool forceUpdate = false)
    {
        if(_currentCharge > MIN_CHARGE && (_prevCurrentCharge == MIN_CHARGE || forceUpdate)) { EnergizeOutCable(true); }
        else if(_currentCharge == MIN_CHARGE && (_prevCurrentCharge > MIN_CHARGE || forceUpdate)) { EnergizeOutCable(false); }
    }
}
