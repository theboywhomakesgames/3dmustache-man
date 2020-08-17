using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bullet : PhysicalObject
{
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
}
