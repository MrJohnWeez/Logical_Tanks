using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Gate : LogicGateBase
{
    [SerializeField] private bool _openRight = true;
    private Animator _animator = null;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        gameManager.OnGameSpeedChanged += UpdateAnimationSpeed;
    }

    protected override void StateSwitched(bool isOn)
    {
        bool isCableEnergized = inCable1 && inCable1.IsEnergized;
        _animator.ResetTrigger("Reset");
        _animator.SetBool("OpenLeft", isCableEnergized && !_openRight);
        _animator.SetBool("OpenRight", isCableEnergized && _openRight);
    }

    public override void ResetObject()
    {
        base.ResetObject();
        bool isCableEnergized = inCable1 && inCable1.IsEnergized;
        _animator.SetBool("OpenLeft", isCableEnergized && !_openRight);
        _animator.SetBool("OpenRight", isCableEnergized && _openRight);
        _animator.ResetTrigger("Reset");
        _animator.SetTrigger("Reset");
    }

    private void UpdateAnimationSpeed(float newSpeed)
    {
        _animator.SetFloat("GameSpeed", newSpeed);
    }
}
