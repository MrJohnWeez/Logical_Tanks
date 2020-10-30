using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ColoredObject
{
    private float _speed = 10.0f; // m/s

    public void IgnoreCollider(Collider ignoreThis)
    {
        Physics.IgnoreCollision(ignoreThis, boxCollider);
    }
    
    protected override void FixedCycle()
    {
        rigidBody.velocity = transform.forward * _speed;
        rigidBody.angularVelocity = Vector3.zero;
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
}
