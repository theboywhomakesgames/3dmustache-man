using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorPlacer : MonoBehaviour
{
    public static Transform indicatorTransform;

    [SerializeField]
    private float _mouseSpeed = 10f, _restrictedRadius = 1f;
    [SerializeField]
    private Transform _dependance, _subPointer;
    [SerializeField]
    private Vector3 _restrictionOffset;

    private Vector3 _subOffset;
    private float _recoilEffectFactor;

    public void ApplyRecoil(float amount)
    {
        _subOffset += Vector3.up * amount * _recoilEffectFactor;
    }

    private void Awake()
    {
        indicatorTransform = transform;
    }

    private void FixedUpdate()
    {
        Vector3 newDiff = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0) * _mouseSpeed;
        _subOffset += newDiff;

        _subPointer.position = _dependance.position + _subOffset;

        Vector3 endResult = _subPointer.position;
        Vector3 diff = _dependance.position + _restrictionOffset - endResult;

        if (diff.magnitude > _restrictedRadius)
        {
            transform.position = endResult;
        }

        _recoilEffectFactor = (transform.position - _dependance.transform.position).magnitude / 4;

        // keep cursur locked and in center
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawSphere(_dependance.position + _restrictionOffset, _restrictedRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, _recoilEffectFactor);
        Gizmos.color = Color.white;
    }
}
