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

    public Action<int> OnTankFinished;
    [HideInInspector] public bool IsReady => _tankState != TankState.Disabled;
    [SerializeField] private GameObject _warningIcon = null;
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
    private NodeManager _nodeManager = null;

    protected override void Awake()
    {
        base.Awake();
        _tankShooter = GetComponent<TankShooter>();
        _nodeManager = GameObject.FindObjectOfType<NodeManager>();
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
                        transform.position = Vector3.Lerp(_oldPosition, _targetPosition, _currentTimer / _maxTimer);
                        break;
                    case TankState.TankRotating:
                        transform.rotation = Quaternion.Lerp(_oldRotation, _targetRotation, _currentTimer / _maxTimer);
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
            if(!WillTankOverlapOtherColliders(_targetPosition, transform.rotation))
            {
                _tankState = TankState.TankMoving;
            }
            else
            {
                TankCollisionError();
            }
        }
        else
        {
            OnTankFinished?.Invoke(0);
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
            if(!WillTankOverlapOtherColliders(transform.position, _targetRotation))
            {
                _tankState = TankState.TankRotating;
            }
            else
            {
                TankCollisionError();
            }
        }
        else
        {
            OnTankFinished?.Invoke(0);
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
            OnTankFinished?.Invoke(0);
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
            OnTankFinished?.Invoke(0);
        }
    }

    public void ResetStateMachineToIdle()
    {
        ResetStateVaribles();
        _tankState = TankState.Idle;
        OnTankFinished?.Invoke(0);
    }

    private void ResetStateVaribles()
    {
        _currentTimer = 0;
        _maxTimer = 0;
        _oldPosition = Vector3.zero;
        _targetPosition = Vector3.zero;
        _oldRotation = Quaternion.identity;
        _targetRotation = Quaternion.identity;
    }

    public override void ResetObject()
    {
        ResetStateMachineToIdle();
        _turret.transform.localRotation = Quaternion.identity;
        _warningIcon.SetActive(false);
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

    private void TankCollisionError()
    {
        ResetStateVaribles();
        _nodeManager.SpawnTankCollisionError();
        _warningIcon.SetActive(true);
        OnTankFinished?.Invoke(1);
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
