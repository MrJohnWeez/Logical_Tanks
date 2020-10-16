using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class TankController : MonoBehaviour
{
    public ColorID GetColorID  => _colorID;
    [SerializeField] private GameObject _tankTurret = null;
    private ColorID _colorID = ColorID.Green;
    private Rigidbody _rigidBody = null;
    private BoxCollider _bocCollider = null;

    private void Awake()
    {
        Material[] materials = _tankTurret.GetComponent<Renderer>().materials;
        foreach(Material mat in materials)
        {
            _colorID = mat.GetMatchingColor();
            if(_colorID != ColorID.None)
                break;
        }
    }

    public IEnumerator Move(bool moveForward, int power, float durration)
    {
        float durrationLeft = durration;
        power *= moveForward ? 1 : -1;
        while(durrationLeft > 0)
        {
            _rigidBody.velocity = transform.forward * (power / 100);
            yield return new WaitForFixedUpdate();
        }
        _rigidBody.velocity = Vector3.zero;
    }

    public void Explode()
    {
        Debug.Log("Tank Exploded with color: " + _colorID);
        Destroy(gameObject);
    }
}
