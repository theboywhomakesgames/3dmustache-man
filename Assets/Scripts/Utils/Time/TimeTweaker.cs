using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTweaker : MonoBehaviour
{
    [SerializeField]
    private float _stepSize = 0.2f, _fastModeFactor = 5;
    private float _beforeFastening = 0;

    protected static float _fixedToScale = 0.01f, _oneMinusTS;
    protected static bool _isFast = false;

    public void FastenUp()
    {
        if (!_isFast && _oneMinusTS > 0)
        {
            _isFast = true;

            _beforeFastening = Time.timeScale;

            Time.timeScale += _fastModeFactor * _oneMinusTS;
            SetFixedDeltaTime();
        }
    }

    public void SlowDown()
    {
        if (_isFast)
        {
            _isFast = false;

            Time.timeScale = _beforeFastening;
            SetFixedDeltaTime();
        }
    }

    private void SetFixedDeltaTime()
    {
        Time.fixedDeltaTime = Time.timeScale * _fixedToScale;
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
            SetFixedDeltaTime();
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
