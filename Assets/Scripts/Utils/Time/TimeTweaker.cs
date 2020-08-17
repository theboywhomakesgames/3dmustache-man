using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTweaker : MonoBehaviour
{
    [SerializeField]
    private float _stepSize = 0.2f;

    protected static float _fixedToScale = 0.01f;
    protected static bool _isSlowed = false, _set = false;

    private void Start()
    {
        if(!_set)
            SetToNormal();
    }

    private void SetToNormal()
    {
        Time.fixedDeltaTime = _fixedToScale;
        Time.timeScale = 1;
    }

    private void Tweak(float delta)
    {
        Time.timeScale = Mathf.Clamp(Time.timeScale - delta * _stepSize, 0.01f, 1);
        Time.fixedDeltaTime = Time.timeScale * _fixedToScale;
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
