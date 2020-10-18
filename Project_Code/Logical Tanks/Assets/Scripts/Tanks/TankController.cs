using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankShooter))]
public class TankController : ColoredObject
{
    [SerializeField] private GameObject _turret = null;
    private const float RELOAD_DELAY = 0.3f;
    private const float MAX_MOVE_SPEED = 2.0f; // m/s
    private const float MAX_TURN_SPEED = 1.0f; // deg/s
    private const float MAX_TURN_TURRET_SPEED = 1.0f; // deg/s
    private bool _isMoving = false;
    private bool _isTurning = false;
    
    private TankShooter _tankShooter = null;

    public override void Awake()
    {
        base.Awake();
        _tankShooter = GetComponent<TankShooter>();
    }

    private void Update()
    {
        if(!_isMoving) { rigidBody.velocity = Vector3.zero; }
        if(!_isTurning) { rigidBody.angularVelocity = Vector3.zero; }
    }

    public IEnumerator MoveTank(float meters)
    {
        Debug.Log("Tank Controller Move()");
        float durrationLeft = meters;
        _isMoving = true;
        while(durrationLeft > 0)
        {
            durrationLeft -= Time.deltaTime;
            rigidBody.velocity = transform.forward * MAX_MOVE_SPEED ;
            yield return new WaitForFixedUpdate();
        }
        _isMoving = false;
    }

    public IEnumerator RotateTank(float degrees)
    {
        Debug.Log("Tank Controller Rotate()");
        float scalar = degrees / 90 / MAX_TURN_SPEED;
        float currentDurration = 0;
        Quaternion oldRotation = rigidBody.rotation;
        Quaternion targetRotation = rigidBody.rotation * Quaternion.AngleAxis(degrees, Vector3.up);
        _isTurning = true;
        while(currentDurration < 1)
        {
            currentDurration += Time.deltaTime / scalar;
            if(currentDurration > 1)
                currentDurration = 1;

            rigidBody.MoveRotation(Quaternion.Lerp(oldRotation, targetRotation, currentDurration));
            yield return new WaitForFixedUpdate();
        }
        _isTurning = false;
    }

    public IEnumerator RotateTurret(float degrees)
    {
        float scalar = degrees / 90 / MAX_TURN_TURRET_SPEED;
        float currentDurration = 0;
        Quaternion oldRotation = _turret.transform.rotation;
        Quaternion targetRotation = _turret.transform.rotation * Quaternion.AngleAxis(degrees, Vector3.up);
        while(currentDurration < 1)
        {
            currentDurration += Time.deltaTime / scalar;
            if(currentDurration > 1)
                currentDurration = 1;

            _turret.transform.rotation = Quaternion.Lerp(oldRotation, targetRotation, currentDurration);
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator ShootTurret()
    {
        _tankShooter.Shoot(boxCollider);
        yield return new WaitForSeconds(RELOAD_DELAY);
    }

    public void Explode()
    {
        Debug.Log("Tank Exploded with color: " + GetColorID);
        Destroy(gameObject);
    }
}
