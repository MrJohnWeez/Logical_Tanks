using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : ColoredObject
{
    public override void ResetObject()
    {
        gameObject.SetActive(true);
        base.ResetObject();
    }

    public override void HitWithBullet(Vector3 position)
    {
        gameObject.SetActive(false);
    }
}
