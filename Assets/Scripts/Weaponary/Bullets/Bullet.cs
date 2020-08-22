using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bullet : PhysicalObject
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private GameObject _bulletImpactEffect;

    public void GetShot(Vector3 dir)
    {
        transform.up = dir;
        _rb.velocity = dir * _speed;
    }

    protected override void Awake()
    {
        base.Awake();
        _rb.useGravity = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        DoImpact(collision.GetContact(0));
    }

    private void DoImpact(ContactPoint cp)
    {
        GameObject bullet_go = Instantiate(_bulletImpactEffect, cp.point, Quaternion.identity);
        bullet_go.transform.up = cp.normal;
        Destroy(gameObject);
    }
}
