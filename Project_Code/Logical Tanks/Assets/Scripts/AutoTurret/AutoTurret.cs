using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AutoTurret : ColoredObject
{
    [Header("AutoTurret")]
    [SerializeField] private float _currentCooldown = 0;
    [SerializeField] private float _fireRate = 2;
    [SerializeField] private GameObject _bulletStart = null;
    [SerializeField] private GameObject _bulletPrefab = null;
    private List<Bullet> _bullets = new List<Bullet>();
    private BoxCollider _boxCollider = null;
    
    protected override void Awake()
    {
        base.Awake();
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if(_currentCooldown >= 0)
        {
            _currentCooldown -= Time.deltaTime;
            if(_currentCooldown < 0)
            {
                _currentCooldown = _fireRate;
                Shoot();
            }
        } 
    }

    public void Shoot()
    {
        GameObject bulletGO = Instantiate(_bulletPrefab, _bulletStart.transform.position, _bulletStart.transform.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.IgnoreCollider(_boxCollider);
        _bullets.Add(bullet);
    }
}
