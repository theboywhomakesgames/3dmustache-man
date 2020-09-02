using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollider : MonoBehaviour
{
    [SerializeField]
    private GameObject _bulletImpactEffect;

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
