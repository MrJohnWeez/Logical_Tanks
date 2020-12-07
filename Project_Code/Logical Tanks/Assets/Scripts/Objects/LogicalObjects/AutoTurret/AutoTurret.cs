using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurret : ColoredObject
{
    [Header("AutoTurret")]
    [SerializeField] private float _currentCooldown = 0;
    [SerializeField] private float _bulletSpeed = 500;
    [SerializeField] private float _fireRate = 2;
    [SerializeField] private GameObject _bulletStart = null;
    [SerializeField] private GameObject _bulletPrefab = null;
    private List<Bullet> _bullets = new List<Bullet>();
    private float _startCooldown = 0;

    protected override void Start()
    {
        base.Start();
        _startCooldown = _currentCooldown;
    }
    
    protected virtual void Update()
    {
        if(_currentCooldown >= 0)
        {
            _currentCooldown -= Time.deltaTime * gameManager.IndirectMultiplier;
            if(_currentCooldown <= 0)
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
        bullet.IgnoreCollider(boxCollider);
        bullet.SetSpeed(_bulletSpeed);
        _bullets.Add(bullet);
    }

    public override void HitWithBullet(Vector3 position)
    {
        Debug.Log("Hit shooter with name: " + name);
        gameObject.SetActive(false);
        base.HitWithBullet(position);
    }

    public override void ResetObject()
    {
        _currentCooldown = _startCooldown;
        _bullets.Clear();
        gameObject.SetActive(true);
    }
}
