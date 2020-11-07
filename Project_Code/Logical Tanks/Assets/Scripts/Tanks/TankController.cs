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
        Shooting
    }

    public Action OnTankStateChangedToIdle;

    [SerializeField] private GameObject _turret = null;
    private const float RELOAD_DELAY = 0.3f;
    private const float MAX_MOVE_SPEED = 1.0f; // m/s
    private const float MAX_ROTATION_SPEED = 90.0f; // deg/s
    private const float MAX_TURN_TURRET_SPEED = 90.0f; // deg/s
    private TankShooter _tankShooter = null;
    private TankState _tankState = TankState.Idle;
    private float _currentTimer = 0;
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

    protected virtual void FixedUpdate()
    {
        if(_tankState != TankState.Idle)
        {
            if(_currentTimer < _maxTimer && rigidBody && _turret)
            {
                _currentTimer += Time.deltaTime * gameManager.GameSpeed;
                _currentTimer = Mathf.Clamp(_currentTimer, 0, _maxTimer);
                if(_tankState == TankState.TankMoving)
                {
                    Vector3 newPos = Vector3.Lerp(_oldPosition, _targetPosition, _currentTimer / _maxTimer);
                    if (!WillTankOverlapOtherColliders(newPos, rigidBody.rotation)) { rigidBody.MovePosition(newPos); }
                }
                else if(_tankState == TankState.TankRotating)
                {
                    Quaternion newRot = Quaternion.Lerp(_oldRotation, _targetRotation, _currentTimer / _maxTimer);
                    if (!WillTankOverlapOtherColliders(rigidBody.position, newRot)) { rigidBody.MoveRotation(newRot); }
                }
                else if(_tankState == TankState.TurretRotating)
                {
                    _turret.transform.rotation = Quaternion.Lerp(_oldRotation, _targetRotation, _currentTimer / _maxTimer);
                }
                else if(_tankState == TankState.Shooting) { }
            }
            else { ResetStateMachine(); }
        }
    }

    public void MoveTank(float meters)
    {
        _tankState = TankState.TankMoving;
        _currentTimer = 0;
        _maxTimer = Mathf.Abs(meters / MAX_MOVE_SPEED);
        _oldPosition = rigidBody.position;
        _targetPosition = rigidBody.position + transform.forward * meters;
    }

    public void RotateTank(float degrees)
    {
        _tankState = TankState.TankRotating;
        _currentTimer = 0;
        _maxTimer = Mathf.Abs(degrees / MAX_ROTATION_SPEED);
        _oldRotation = rigidBody.rotation;
        _targetRotation = rigidBody.rotation * Quaternion.AngleAxis(degrees, Vector3.up);
    }
    
    public void RotateTurret(float degrees)
    {
        _tankState = TankState.TurretRotating;
        _currentTimer = 0;
        _maxTimer = Mathf.Abs(degrees / MAX_TURN_TURRET_SPEED);
        _oldRotation = _turret.transform.rotation;
        _targetRotation = _turret.transform.rotation * Quaternion.AngleAxis(degrees, Vector3.up);
    }

    public void ShootTurret()
    {
        _tankState = TankState.Shooting;
        _currentTimer = 0;
        _maxTimer = RELOAD_DELAY;
        _tankShooter.Shoot(boxCollider);
    }

    public void ResetStateMachine()
    {
        _tankState = TankState.Idle;
        _currentTimer = 0;
        _maxTimer = 0;
        _oldPosition = Vector3.zero;
        _targetPosition = Vector3.zero;
        _oldRotation = Quaternion.identity;
        _targetRotation = Quaternion.identity;
        OnTankStateChangedToIdle?.Invoke();
    }

    public override void ResetObject()
    {
        ResetStateMachine();
        base.ResetObject();
    }

    public override void HitWithBullet(Vector3 position)
    {
        Debug.Log("Tank Exploded with color: " + GetColorID);
        gameObject.SetActive(false);
        base.HitWithBullet(position);
    }

    private bool WillTankOverlapOtherColliders(Vector3 position, Quaternion rotation)
    {
        Vector3 center = boxCollider.transform.TransformPoint(boxCollider.center);
        center -= transform.position;
        center += position;
        Collider[] hitObjects = Physics.OverlapBox(center, boxCollider.size / 2, rotation, ~0, QueryTriggerInteraction.Ignore);
        foreach(Collider c in hitObjects)
        {
            if(!c.Equals(boxCollider))
            {
                return true;
            }
        }
        return false;
    }
}
