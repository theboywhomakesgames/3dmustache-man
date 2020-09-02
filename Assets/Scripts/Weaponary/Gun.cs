using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Gun : PickUpable
{
    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private AudioClip _shootingSFX;
    [SerializeField]
    private Transform _gunHole;
    [SerializeField]
    private float _fixedZ = 0.5f, _restDuration = 0.5f, _recoilAmount = 0.1f, _shakeStrength = 5, _reloadDuration, _spread, _soundRadius = 3;
    [SerializeField]
    private int _clipSize = 12, _curClip, _curClips = 3, _chunckSize = 1;
    [SerializeField]
    private string _name = "pistol";
    [SerializeField]
    private bool _loadOnStart = true;

    private float _time;
    private bool isResting = false, _isOut = false;
    private IndicatorPlacer _indicator;
    private AudioSource _audioSource;

    public void ShootOne(Vector3 dir)
    {
        dir.z = 0;
        GameObject go = Instantiate(_bulletPrefab, new Vector3(_gunHole.position.x, _gunHole.position.y, _fixedZ), Quaternion.identity);
        Bullet b = go.GetComponent<Bullet>();
        b.GetShot(dir);
    }

    public override void Trigger(Vector3 dir)
    {
        if (!isResting && !_isOut)
        {
            isResting = true;

            for(int bullIndex = 0; bullIndex < _chunckSize; bullIndex++)
            {
                int variationIdx = -_chunckSize / 2 + bullIndex;
                Vector3 newDir = Quaternion.Euler(0, 0, _spread * variationIdx) * dir;
                ShootOne(newDir);
            }

            _indicator.ApplyRecoil(_recoilAmount);
            ScreeShake();
            _audioSource.PlayOneShot(_shootingSFX);

            _curClip--;
            if(_curClip <= 0)
            {
                GoOut();
            }

            int mask = 1 << 10;
            Collider[] colliders = Physics.OverlapSphere(transform.position, _soundRadius, mask);
            foreach(Collider c in colliders)
            {
                try
                {
                    c.gameObject.GetComponent<FollowerEnemy>().HearShit(transform.position);
                }
                catch
                {
                    continue;
                }
            }
        }
    }

    private void GoOut()
    {
        _isOut = true;

        if(_curClips > 0)
            Invoke(nameof(Reload), _reloadDuration);
    }

    private void Reload()
    {
        _curClip += _clipSize;
        _curClips--;
        _isOut = false;
    }

    private void ScreeShake()
    {
        CameraManager.cams[0].DOShakePosition(_restDuration, strength: _shakeStrength);
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
        _audioSource = GetComponent<AudioSource>();
        _indicator = IndicatorPlacer.indicatorTransform.GetComponent<IndicatorPlacer>();

        if (_loadOnStart)
        {
            Reload();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _soundRadius);
    }
}
