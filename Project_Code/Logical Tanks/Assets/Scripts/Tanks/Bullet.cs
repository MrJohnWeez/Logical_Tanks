using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ColoredObject
{
    private float _speed = 500.0f;

    public void IgnoreCollider(Collider ignoreThis)
    {
        Physics.IgnoreCollision(ignoreThis, boxCollider);
    }
    
    protected virtual void FixedUpdate()
    {
        rigidBody.velocity = transform.forward * _speed * Time.deltaTime * gameManager.GameSpeed;
        rigidBody.angularVelocity = Vector3.zero;
    }

    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        ColoredObject coloredObject = other.gameObject.GetComponent<ColoredObject>();
        if(coloredObject && coloredObject.GetColorID != GetColorID)
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    public override void ResetObject()
    {
        base.ResetObject();
        Destroy(gameObject);
    }
}
