using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(TankShooter))]
public class TankController : ColoredObject
{
    public enum TankState
    {
        Idle,
        TankMoving,
        TankRotating,
        TurretRotating,
        Shooting,
        Disabled
    }

    public Action OnTankFinished;
    [HideInInspector] public bool IsReady => _tankState != TankState.Disabled;
    [SerializeField] private GameObject _turret = null;
    [SerializeField] private LayerMask _moveDetectionHits;
    private const float RELOAD_DELAY = 0.3f;
    private const float MAX_MOVE_SPEED = 1.0f; // m/s
    private const float MAX_ROTATION_SPEED = 90.0f; // deg/s
    private const float MAX_TURN_TURRET_SPEED = 90.0f; // deg/s
    private TankShooter _tankShooter = null;
    private TankState _tankState = TankState.Idle;
    private float _currentTimer = 0;
    private float _prevCurrentTimer = 0;
    private float _maxTimer = 0;
    private Vector3 _oldPosition;
    private Vector3 _targetPosition;
    private Quaternion _oldRotation;
    private Quaternion _targetRotation;

    protected override void Awake()
    {
        base.Awake();
        _tankShooter = GetComponent<TankShooter>();
    }

    private void Update()
    {
        if(_tankState != TankState.Idle && _tankState != TankState.Disabled)
        {
            if(_currentTimer < _maxTimer && _turret)
            {
                float timerAddition = Time.deltaTime * gameManager.IndirectMultiplier;
                _currentTimer = Mathf.Clamp(_currentTimer + timerAddition, 0, _maxTimer);
                switch(_tankState)
                {
                    case TankState.TankMoving:
                        Vector3 newPos = Vector3.Lerp(_oldPosition, _targetPosition, _currentTimer / _maxTimer);
                        if (!WillTankOverlapOtherColliders(newPos, transform.rotation)) { transform.position = newPos; }
                        else { ResetStateMachineToIdle(); }
                        break;
                    case TankState.TankRotating:
                        Quaternion newRot = Quaternion.Lerp(_oldRotation, _targetRotation, _currentTimer / _maxTimer);
                        if (!WillTankOverlapOtherColliders(transform.position, newRot)) { transform.rotation = newRot; }
                        else { ResetStateMachineToIdle(); }
                        break;
                    case TankState.TurretRotating:
                        _turret.transform.localRotation = Quaternion.Lerp(_oldRotation, _targetRotation, _currentTimer / _maxTimer);
                        break;
                    case TankState.Shooting:
                        break;
                }
                _prevCurrentTimer = _currentTimer;
            }
            else if(_currentTimer >= _maxTimer || _turret == null)
            {
                ResetStateMachineToIdle();
            }
        }
    }

    public void MoveTank(float meters)
    {
        if(_tankState == TankState.Idle)
        {
            _oldPosition = transform.position;
            _targetPosition = transform.position + transform.forward * meters;
            _currentTimer = 0;
            _maxTimer = Mathf.Abs(meters / MAX_MOVE_SPEED);
            _tankState = TankState.TankMoving;
        }
        else
        {
            Debug.Log("Skipped!");
            OnTankFinished?.Invoke();
        }
    }

    public void RotateTank(float degrees)
    {
        if(_tankState == TankState.Idle)
        {
            _currentTimer = 0;
            _maxTimer = Mathf.Abs(degrees / MAX_ROTATION_SPEED);
            _oldRotation = transform.rotation;
            _targetRotation = transform.rotation * Quaternion.AngleAxis(degrees, Vector3.up);
            _tankState = TankState.TankRotating;
        }
        else
        {
            Debug.Log("Skipped!");
            OnTankFinished?.Invoke();
        }
    }
    
    public void RotateTurret(float degrees)
    {
        if(_tankState == TankState.Idle)
        {
            _currentTimer = 0;
            _maxTimer = Mathf.Abs(degrees / MAX_TURN_TURRET_SPEED);
            _oldRotation = _turret.transform.localRotation;
            _targetRotation = _turret.transform.localRotation * Quaternion.AngleAxis(degrees, Vector3.up);
            _tankState = TankState.TurretRotating;
        }
        else
        {
            Debug.Log("Skipped!");
            OnTankFinished?.Invoke();
        }
    }

    public void ShootTurret()
    {
        if(_tankState == TankState.Idle)
        {
            _currentTimer = 0;
            _maxTimer = RELOAD_DELAY;
            _tankShooter.Shoot(boxCollider);
            _tankState = TankState.Shooting;
        }
        else
        {
            Debug.Log("Skipped!");
            OnTankFinished?.Invoke();
        }
    }

    public void ResetStateMachineToIdle()
    {
        _currentTimer = 0;
        _maxTimer = 0;
        _oldPosition = Vector3.zero;
        _targetPosition = Vector3.zero;
        _oldRotation = Quaternion.identity;
        _targetRotation = Quaternion.identity;
        _tankState = TankState.Idle;
        OnTankFinished?.Invoke();
    }

    public override void ResetObject()
    {
        ResetStateMachineToIdle();
        _turret.transform.localRotation = Quaternion.identity;
        gameObject.SetActive(true);
        base.ResetObject();
    }

    public override void HitWithBullet(Vector3 position)
    {
        gameObject.transform.position -= Vector3.up * 100;
        gameObject.SetActive(false);
        ResetStateMachineToIdle();
        base.HitWithBullet(position);
        _tankState = TankState.Disabled;
    }

    private bool WillTankOverlapOtherColliders(Vector3 position, Quaternion rotation)
    {
        Vector3 center = boxCollider.transform.TransformPoint(boxCollider.center);
        center -= transform.position;
        center += position;
        Collider[] hitObjects = Physics.OverlapBox(center, boxCollider.size / 2, rotation, _moveDetectionHits, QueryTriggerInteraction.Ignore);
        foreach(Collider c in hitObjects)
        {
            if(!c.Equals(boxCollider)) { return true; }
        }
        return false;
    }
}
