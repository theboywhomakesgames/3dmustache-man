using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class FollowerEnemy : EnemyController
{
    public Vector3 targetPos;
    public Transform target;

    private bool _isSuspiciousAboutPosition;
    private bool _isGoing, _shouldBeGoing, _shouldInteract;

    private void Start()
    {
        GotoPosition(target.position);
    }

    private void GotoPosition(Vector3 position)
    {
        _shouldBeGoing = true;
        _shouldInteract = false;
        targetPos = position;
    }

    private void GotoAndInteract(Vector3 position)
    {
        _shouldInteract = true;
        GotoPosition(position);
    }

    private void Update()
    {
        Vector3 diff = targetPos - transform.position;
        if (_shouldBeGoing && !_isGoing)
        {
            _character.AimAt(targetPos);
            if(diff.x > 0)
            {
                _character.StartMovingRight();
            }
            else
            {
                _character.StartMovingLeft();
            }
            _isGoing = true;
            _shouldBeGoing = false;
        }

        if (_isGoing)
        {
            if (Mathf.Abs(diff.x) > _character.HandReach)
                _character.Move((int)Mathf.Sign(diff.x));
            else
            {
                _isGoing = false;
                if (diff.x > 0)
                {
                    _character.StopMovingRight();
                }
                else
                {
                    _character.StopMovingLeft();
                }

                if (_shouldInteract)
                {
                    _character.InteractWithNearby();
                }
            }
        }
    }
}
