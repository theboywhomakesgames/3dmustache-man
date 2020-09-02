using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class FollowerEnemy : EnemyController
{
    public Vector3 targetPos;
    public Transform target;

    [SerializeField]
    private bool _isSuspiciousAboutPosition, _followingPath, _hasShaash;
    [SerializeField]
    private bool _isGoing, _shouldBeGoing, _shouldInteract;

    private List<Vector3> _path;

    // TODO:
    // assign pathfinder dynamically
    [SerializeField]
    private Pathfinder _pathfinder;

    public void HearShit(Vector3 from)
    {
        _hasShaash = true;
        GotoTarget(from);
    }

    private void Start()
    {

    }

    private void GotoTarget(Vector3 gotoPos)
    {
        _path = _pathfinder.FindAPath(transform.position, gotoPos);
        // TODO:
        // bring this line to pathfinder
        _path.Reverse();
        if(_path.Count < 1)
        {
            print("no path");
        }
        else
        {
            FollowPath();
        }
    }

    private void FollowPath()
    {
        GotoPosition(_path[0]);
        _path.RemoveAt(0);
        
        _followingPath = _path.Count > 0;        
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
        Vector3 diff = targetPos - _character.Center;
        if (_shouldBeGoing && !_isGoing)
        {
            SetRunningStatus();

            _character.AimAt(targetPos);
            if (diff.x > 0)
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
            if (diff.magnitude > _character.HandReach)
            {
                _character.Move((int)Mathf.Sign(diff.x));
            }
            else
            {
                _isGoing = false;

                if (_shouldInteract)
                {
                    TryToStopMoving(diff);
                    _character.InteractWithNearby();
                }

                if (_followingPath)
                {
                    FollowPath();
                }
                else
                {
                    TryToStopMoving(diff);
                }
            }
        }
    }

    private void SetRunningStatus()
    {
        if (_hasShaash != _character.isRunning)
        {
            _character.ToggleRun();
        }
    }

    private void TryToStopMoving(Vector3 diff)
    {
        _hasShaash = false;

        SetRunningStatus();
        _character.StopMovingRight();
        _character.StopMovingLeft();
    }
}
