using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankShooter))]
public class TankController : ColoredObject
{
    [SerializeField] private GameObject _turret = null;
    private const float RELOAD_DELAY = 0.3f;
    private bool _isMoving = false;
    private bool _isTurning = false;
    private float _maxMoveSpeed = 2.0f; // m/s
    private float _maxTurnSpeed = 1.0f; // deg/s
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

    public IEnumerator Move(bool moveForward, int power, float durration)
    {
        Debug.Log("Tank Controller Move()");
        float durrationLeft = durration;
        power *= moveForward ? 1 : -1;
        _isMoving = true;
        float speed = power / 100.0f * _maxMoveSpeed;
        while(durrationLeft > 0)
        {
            durrationLeft -= Time.deltaTime;
            rigidBody.velocity = transform.forward * speed;
            yield return new WaitForFixedUpdate();
        }
        _isMoving = false;
    }

    public IEnumerator Turn(bool turnRight, int degrees, float durration)
    {
        // Debug.Log("Tank Controller Move()");
        // float durrationLeft = durration;
        // degrees *= turnRight ? 1 : -1;
        // _isTurning = true;
        // float speed = power / 100.0f * _maxMoveSpeed;
        // while(durrationLeft > 0)
        // {
        //     durrationLeft -= Time.deltaTime;
        //     rigidBody.velocity = transform.forward * speed;
        //     yield return new WaitForFixedUpdate();
        // }
        // _isTurning = false;
        yield return new WaitForFixedUpdate();
    }

    public IEnumerator TurnTurret(bool turnRight, int degrees, float durration)
    {
        Debug.Log("Tank Controller TurnTurret()");
        float currentDurration = durration;
        degrees *= turnRight ? 1 : -1;
        while(currentDurration > 0)
        {
            float turnAmount = Mathf.Lerp(0, (float)degrees, currentDurration);
            _turret.transform.Rotate(transform.up, turnAmount, Space.Self);
            currentDurration += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator Shoot()
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
