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

    private void Start()
    {
        GameObject lineRenderGO = Instantiate(_runtimeRayPrefab, _bulletStart.transform);
        lineRenderGO.transform.position = _bulletStart.transform.position;
        lineRenderGO.transform.rotation = _bulletStart.transform.rotation;
        _lineRenderRay = lineRenderGO.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Physics.Raycast(_bulletStart.transform.position, _bulletStart.transform.forward, out hit, Mathf.Infinity, _hitMask))
        {
            _lineRenderRay.SetPosition(0, hit.point);
            _lineRenderRay.SetPosition(1, _bulletStart.transform.position);
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
