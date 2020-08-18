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
    private float _fixedZ = 0.5f, _restDuration = 0.5f, _recoilAmount = 0.1f;

    private float _time;
    private bool isResting = false;
    private IndicatorPlacer _indicator;

    public void ShootOne(Vector3 dir)
    {
        dir.z = 0;
        GameObject go = Instantiate(_bulletPrefab, new Vector3(_gunHole.position.x, _gunHole.position.y, _fixedZ), Quaternion.identity);
        Bullet b = go.GetComponent<Bullet>();
        b.GetShot(dir);
    }

    public override void Trigger(Vector3 dir)
    {
        if (!isResting)
        {
            isResting = true;
            ShootOne(dir);
            _indicator.ApplyRecoil(_recoilAmount);
        }
    }

    private void Update()
    {
        if (isResting)
        {
            _time += Time.deltaTime;
            if(_time > _restDuration)
            {
                _time = 0;
                isResting = false;
            }
        }
    }

    private void Start()
    {
        _indicator = IndicatorPlacer.indicatorTransform.GetComponent<IndicatorPlacer>();
    }
}
