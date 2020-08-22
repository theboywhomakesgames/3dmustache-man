using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSensor : MonoBehaviour
{
    public delegate void SensorEventHandler(Transform intruder);

    public event SensorEventHandler OnEnter, OnExit, OnStay;

    [SerializeField]
    protected bool _isTrigger;

    protected void OnCollisionEnter(Collision collision)
    {
        if (!_isTrigger)
        {
            OnEnter?.Invoke(collision.transform);
        }
    }

    protected void OnCollisionExit(Collision collision)
    {
        if (!_isTrigger)
        {
            OnExit?.Invoke(collision.transform);
        }
    }

    protected void OnCollisionStay(Collision collision)
    {
        if (!_isTrigger)
        {
            OnStay?.Invoke(collision.transform);
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (_isTrigger)
        {
            OnEnter?.Invoke(other.transform);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (_isTrigger)
        {
            OnExit?.Invoke(other.transform);
        }
    }

    protected void OnTriggerStay(Collider other)
    {
        if (_isTrigger)
        {
            OnStay?.Invoke(other.transform);
        }
    }
}
