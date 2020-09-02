using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bullet : PhysicalObject
{
    public float damage = 100;

    [SerializeField]
    private float _speed;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 || other.gameObject.layer == 10)
        {
            other.gameObject.GetComponent<Person>().ReceiveDamage(damage, _rb.velocity, transform.position);
        }
    }
}
