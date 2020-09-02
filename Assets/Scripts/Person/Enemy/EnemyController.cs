using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Person))]
public class EnemyController : MonoBehaviour
{
    protected Person _character;
    protected bool _isAlert, _hasTarget, _isShooting;

    [SerializeField]
    protected SimpleSensor _sensor;

    protected Transform _threat;

    // TODO:
    // assign pathfinder dynamically
    [SerializeField]
    protected Pathfinder _pathfinder;

    public void SetPathfinder(Pathfinder pf)
    {
        _pathfinder = pf;
    }

    protected virtual void Awake()
    {
        _character = GetComponent<Person>();
        _sensor.OnEnter += OnSensorEnter;
        _sensor.OnExit += OnSensorExit;
    }

    // problematic when using multiple colliders on one person
    private void OnSensorEnter(Transform intruder)
    {
        if(intruder.gameObject.layer == 9 && !_hasTarget)
        {
            _hasTarget = true;
            _threat = intruder;
        }
    }

    private void OnSensorExit(Transform intruder)
    {
        if (intruder.gameObject.layer == 9 && _hasTarget)
        {
            _hasTarget = false;
        }
    }

    protected virtual void Update()
    {
        // should be alert?!
        if(_hasTarget != _isAlert)
        {
            _isAlert = _hasTarget;
            if (_isAlert)
                Invoke(nameof(StartShooting), 1);
        }

        // what to do?
        if (_hasTarget)
        {
            _character.AimAt(_threat.position);

            if (_isShooting)
            {
                _character.Trigger();
            }
        }
    }

    private void StartShooting()
    {
        _isShooting = true;
    }
}