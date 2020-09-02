using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTweaker : MonoBehaviour
{
    [SerializeField]
    private float _stepSize = 0.2f, _fastModeFactor = 5;
    private float _beforeFastening = 0;

    protected static float _fixedToScale = 0.01f, _oneMinusTS, _curTS;
    protected static bool _isFast = false;

    protected Tween dtTween, fixeddtTween;

    public void FastenUp()
    {
        if (!_isFast && _oneMinusTS > 0)
        {
            _isFast = true;

            _beforeFastening = Time.timeScale;

            try
            {
                dtTween.Kill();
                fixeddtTween.Kill();
            }
            catch { }

            _curTS = Time.timeScale + _fastModeFactor * _oneMinusTS;
            DOTween.To(()=>Time.timeScale, (x)=> { Time.timeScale = x; }, Time.timeScale + _fastModeFactor * _oneMinusTS, 0.2f).SetUpdate(true);
            SetFixedDeltaTime();
        }
    }

    public void SlowDown()
    {
        if (_isFast)
        {
            _isFast = false;

            try
            {
                dtTween.Kill();
                fixeddtTween.Kill();
            }
            catch { }

            _curTS = _beforeFastening;
            DOTween.To(() => Time.timeScale, (x) => { Time.timeScale = x; }, _beforeFastening, 0.2f).SetUpdate(true);
            SetFixedDeltaTime();
        }
    }

    private void SetFixedDeltaTime()
    {
        DOTween.To(() => Time.fixedDeltaTime, (x) => { Time.fixedDeltaTime = x; }, _curTS * _fixedToScale, 0.2f).SetUpdate(true);
    }

    private void Start()
    {
        SetToNormal();
    }

    private void SetToNormal()
    {
        Time.fixedDeltaTime = _fixedToScale;
        Time.timeScale = 1;
        _oneMinusTS = 1 - Time.timeScale;
    }

    private void Tweak(float delta)
    {
        if (!_isFast)
        {
            Time.timeScale = Mathf.Clamp(Time.timeScale - delta * _stepSize, 0.01f, 1);
            Time.fixedDeltaTime = Time.timeScale * _fixedToScale;
            _curTS = Time.timeScale;
            _oneMinusTS = 1 - Time.timeScale;
        }
    }

    private void Update()
    {
        Vector2 mw = Input.mouseScrollDelta;
        if(mw.y != 0)
        {
            Tweak(mw.y);
        }
    }
}
