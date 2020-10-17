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
    private bool _isMoving = false;
    private bool _isTurning = false;
    private float _maxMoveSpeed = 2.0f; // m/s
    private float _maxTurnSpeed = 1.0f; // deg/s
    
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _bocCollider = GetComponent<BoxCollider>();
        Material[] materials = _tankTurret.GetComponent<Renderer>().materials;
        foreach(Material mat in materials)
        {
            _colorID = mat.GetMatchingColor();
            if(_colorID != ColorID.None)
                break;
        }
    }

    private void Update()
    {
        if(!_isMoving) { _rigidBody.velocity = Vector3.zero; }
        if(!_isTurning) { _rigidBody.angularVelocity = Vector3.zero; }
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
            _rigidBody.velocity = transform.forward * speed;
            yield return new WaitForFixedUpdate();
        }
        _isMoving = false;
    }

    public void Explode()
    {
        Debug.Log("Tank Exploded with color: " + _colorID);
        Destroy(gameObject);
    }
}
