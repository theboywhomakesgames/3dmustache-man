using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public Vector3 size, offset;
    
    public int layerMask;

    [SerializeField]
    private float _unitOffset, _unitRadius, _stepSize;

    private bool[,] _units;
    private bool _learned, _isRunning;

    [SerializeField]
    private float _time, _betweenUpdates;
    private bool _isResting;

    public void Learn()
    {
        _learned = true;
        float x = _stepSize == 1 ? size.x : (size.x / _stepSize);
        float y = _stepSize == 1 ? size.y : (size.y / _stepSize);

        _units = new bool[(int)Mathf.Ceil(x), (int)Mathf.Ceil(y)];

        print(_learned);
        print(_units.GetLength(0) + " " + _units.GetLength(1));
    }

    private void PhysicalLearn()
    {
        int x = 0, y = 0;
        for (float xOffset = -size.x / 2; xOffset < size.x / 2; xOffset += _stepSize)
        {
            y = 0;
            for (float yOffset = -size.y / 2; yOffset < size.y / 2; yOffset += _stepSize)
            {
                _units[x, y] = Physics.CheckSphere(
                    transform.position + offset + new Vector3(xOffset, yOffset),
                    _unitRadius - _unitOffset,
                    layerMask
                );
                y++;
            }
            x++;
        }
    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position + offset, size);

        if(_stepSize > 0.05 && _isRunning)
        {
            int x = 0, y = 0;
            for(float xOffset = -size.x/2; xOffset < size.x/2; xOffset += _stepSize){
                y = 0;
                for(float yOffset = -size.y/2; yOffset < size.y/2; yOffset += _stepSize){
                    if(_units[x, y])
                    {
                        Gizmos.color = Color.black;
                    }
                    else
                    {
                        Gizmos.color = Color.white;
                    }

                    Gizmos.DrawWireSphere(
                        transform.position + offset + new Vector3(xOffset, yOffset),
                        _unitRadius - _unitOffset
                    );
                    y++;
                }
                x++;
            }
        }
    }

    private void Awake()
    {
        if(!_learned)
            Learn();

        _isRunning = true;
    }

    private void Update()
    {
        if (_isResting)
        {
            _time += Time.deltaTime;
            if(_time > _betweenUpdates)
            {
                _isResting = false;
                _time = 0;
            }
        }
        else
        {
            _isResting = true;
            Invoke(nameof(PhysicalLearn), 0);
        }
    }
}
