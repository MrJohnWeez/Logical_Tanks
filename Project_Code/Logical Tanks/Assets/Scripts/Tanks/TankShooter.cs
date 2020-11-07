using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooter : MonoBehaviour
{
    [SerializeField] private GameObject _bulletStart = null;
    [SerializeField] private GameObject _bulletPrefab = null;
    [SerializeField] private LayerMask _hitMask;
    [SerializeField] private GameObject _runtimeRayPrefab = null;
    private LineRenderer _lineRenderRay = null;
    private RaycastHit hit;
    private Vector3 forwardDirection;

    private void Start()
    {
        GameObject lineRenderGO = Instantiate(_runtimeRayPrefab, _bulletStart.transform);
        lineRenderGO.transform.position = _bulletStart.transform.position;
        lineRenderGO.transform.rotation = _bulletStart.transform.rotation;
        _lineRenderRay = lineRenderGO.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        // Does the ray intersect any objects excluding the player layer
        forwardDirection = _bulletStart.transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(_bulletStart.transform.position, forwardDirection, out hit, Mathf.Infinity, _hitMask))
        {
            _lineRenderRay.SetPosition(0, hit.point);
            _lineRenderRay.SetPosition(1, hit.point - forwardDirection * hit.distance);
        }
    }

    public void Shoot(Collider ignoreThis = null)
    {
        GameObject bulletGO = Instantiate(_bulletPrefab, _bulletStart.transform.position, _bulletStart.transform.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if(ignoreThis)
            bullet.IgnoreCollider(ignoreThis);
    }
}
