using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooter : MonoBehaviour
{
    [SerializeField] private GameObject _bulletStart = null;
    [SerializeField] private GameObject _bulletPrefab = null;
    [SerializeField] private LayerMask _hitMask;
    private List<Bullet> _bullets = new List<Bullet>();

    private void Update()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(_bulletStart.transform.position, _bulletStart.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, _hitMask))
        {
            Debug.DrawRay(_bulletStart.transform.position, _bulletStart.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
        }
    }

    public void Shoot(Collider ignoreThis = null)
    {
        GameObject bulletGO = Instantiate(_bulletPrefab, _bulletStart.transform.position, _bulletStart.transform.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if(ignoreThis)
            bullet.IgnoreCollider(ignoreThis);

        _bullets.Add(bullet);
    }
}
