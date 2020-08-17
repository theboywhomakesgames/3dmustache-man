using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : PickUpable
{
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private Transform _gunHole;
    [SerializeField]
    private float _fixedZ = 0.5f;

    public void ShootOne(Vector3 dir)
    {
        GameObject go = Instantiate(_bulletPrefab, new Vector3(_gunHole.position.x, _gunHole.position.y, _fixedZ), Quaternion.identity);
        Bullet b = go.GetComponent<Bullet>();
        b.GetShot(dir);
    }

    public override void Trigger(Vector3 dir)
    {
        ShootOne(dir);
    }
}
