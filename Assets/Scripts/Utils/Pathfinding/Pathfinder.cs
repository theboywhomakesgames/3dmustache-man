using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public Vector3 size, offset;
    
    public int layerMask;

    // for testing purposes
    public Transform start, end;

    [SerializeField]
    private float _unitOffset, _unitRadius, _stepSize;

    private byte[,] _units;
    private bool _learned, _isRunning;

    [SerializeField]
    private float _time, _betweenUpdates;
    private bool _isResting;

    public void Learn()
    {
        _learned = true;
        float x = _stepSize == 1 ? size.x : (size.x / _stepSize);
        float y = _stepSize == 1 ? size.y : (size.y / _stepSize);

        _units = new byte[(int)Mathf.Ceil(x), (int)Mathf.Ceil(y)];

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
                ) ? (byte)0x01 : (byte)0x00;
                y++;
            }
            x++;
        }

        FindPath(start.position, end.position);
    }

    private void FindPath(Vector3 start, Vector3 end)
    {
        int xStart, yStart, xEnd, yEnd;

        GetRelativeXY(start, out xStart, out xEnd);
        GetRelativeXY(end, out yStart, out yEnd);

        // convert if elses to calculations if needed optimization
        // convert to if else for optimazation
        try
        {
            _units[xStart, xEnd] = (byte)(0x02 | _units[xStart, xEnd]);
            _units[yStart, yEnd] = (byte)(0x02 | _units[yStart, yEnd]);
        }
        catch { }
    }

    private Vector3 GetRelativeXY(Vector3 start, out int xStart, out int yStart)
    {
        start -= transform.position + offset + new Vector3(-size.x / 2, -size.y / 2);
        xStart = (int)Mathf.Ceil((start.x - _unitRadius) / _stepSize);
        yStart = (int)Mathf.Ceil((start.y - _unitRadius) / _stepSize);
        return start;
    }

    private void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position + offset, size);

        if(_stepSize > 0.05 && _isRunning)
        {
            int x = 0, y = 0;
            for(float xOffset = -size.x/2; xOffset < size.x/2; xOffset += _stepSize){
                y = 0;
                for(float yOffset = -size.y/2; yOffset < size.y/2; yOffset += _stepSize){
                    if((_units[x, y] & 0x01) > 0)
                    {
                        Gizmos.color = Color.red;
                    }
                    else
                    {
                        if ((_units[x, y] & 0x02) > 0)
                        {
                            Gizmos.color = Color.green;
                        }
                        else
                        {
                            Gizmos.color = Color.black;
                        }
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
