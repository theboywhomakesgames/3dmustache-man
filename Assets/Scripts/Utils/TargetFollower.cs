using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class TargetFollower : MonoBehaviour
{
    public Transform target;
    public float width, height, duration = 1;
    public Vector3 offset;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }

    private void Update()
    {
        Vector3 diff = target.position - transform.position;
        if (Mathf.Abs(diff.x) > width / 2 || Mathf.Abs(diff.y) > height / 2)
        {
            transform.DOMove(new Vector3(target.position.x, target.position.y, transform.position.z) + offset, duration);
        }
    }
}
