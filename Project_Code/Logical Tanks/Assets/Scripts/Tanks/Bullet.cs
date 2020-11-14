using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : VariableCycledObject
{
    [SerializeField] private Rigidbody _rigidBody = null;
    [SerializeField] private BoxCollider _boxCollider = null;
    private float _speed = 500.0f;

    public void IgnoreCollider(Collider ignoreThis) { Physics.IgnoreCollision(ignoreThis, _boxCollider); }

    protected virtual void FixedUpdate()
    {
        _rigidBody.velocity = transform.forward * _speed * Time.deltaTime * gameManager.GameSpeed;
        _rigidBody.angularVelocity = Vector3.zero;
    }

    public void SetSpeed(float newSpeed) { _speed = newSpeed; }

    private void OnCollisionEnter(Collision other)
    {
        ColoredObject coloredObject = other.gameObject.GetComponent<ColoredObject>();
        if (coloredObject) { coloredObject.HitWithBullet(gameObject.transform.position); }
        Destroy(gameObject);
    }

    public override void ResetObject() { Destroy(gameObject); }
}
