using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativePlacer : MonoBehaviour
{
    [SerializeField]
    private Vector3 _offset;
    [SerializeField]
    private Transform _base;

    private void Start()
    {
        _offset = transform.position - _base.position;
    }

    private void LateUpdate()
    {
        Vector3 rayPos = _base.position + _offset;

        RaycastHit hit;
        Physics.Raycast(rayPos, Vector3.down, out hit);

        transform.position = hit.point;
    }
}
