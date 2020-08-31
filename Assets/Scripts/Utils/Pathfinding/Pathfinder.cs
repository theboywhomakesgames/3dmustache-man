using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct PathPoint
{
    public int x, y;
    public float score;

    public PathPoint(float score, int x, int y)
    {
        this.score = score;
        this.x = x;
        this.y = y;
    }
}

public struct Unit
{
    public bool visited;
    public int cameFromX, cameFromY;
    public float scoreSoFar;
}

public class FloatComparer : IComparer<float>
{
    public int Compare(float i1, float i2)
    {
        return i1 > i2 ? 1 : -1;
    }
}

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
    private bool _isResting, _hasGravity;

    [SerializeField]
    private Vector2Int[] _moves;

    private SortedList<float, PathPoint> _options;
    private PathPoint _bestSoFar;

    public void Learn()
    {
        _learned = true;
        float x = _stepSize == 1 ? size.x : (size.x / _stepSize);
        float y = _stepSize == 1 ? size.y : (size.y / _stepSize);

        _units = new byte[(int)Mathf.Ceil(x), (int)Mathf.Ceil(y)];
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

        GetRelativeXY(start, out xStart, out yStart);
        GetRelativeXY(end, out xEnd, out yEnd);

        // convert if elses to calculations if needed optimization
        // convert to if else for optimazation
        try
        {
            _units[xStart, yStart] = (byte)(0x02 | _units[xStart, yStart]);

            Unit[,] _myUnits = new Unit[_units.GetLength(0), _units.GetLength(1)];
            _myUnits[xStart, yStart].visited = true;
            _myUnits[xStart, yStart].cameFromX = xStart;
            _myUnits[xStart, yStart].cameFromY = yStart;
            _myUnits[xStart, yStart].scoreSoFar = Mathf.Abs(xStart - xEnd) + Mathf.Abs(yStart - yEnd);

            _bestSoFar = new PathPoint(_myUnits[xStart, yStart].scoreSoFar, xStart, yStart);

            // shouldn't do this you know ...
            _units[xEnd, yEnd] = (byte)(0x02 | _units[xEnd, yEnd]);

            _options = new SortedList<float, PathPoint>(new FloatComparer());
            AStar(xStart, yStart, xStart, yStart, xEnd, yEnd, _myUnits);
        }
        catch { }
    }

    // TODO:
    // make it a big while() instead of recursive function
    private void AStar(int xs, int ys, int x, int y, int xe, int ye, Unit[,] _myUnits)
    {
        int xx = 0, yy = 0;
        CheckMoves(x, y, xe, ye, _myUnits, ref xx, ref yy);

        if (_options.Count > 0)
        {
            PathPoint p = _options.First().Value;
            _options.RemoveAt(0);

            //if(p.score < _bestSoFar.score)
            //{
            //    _bestSoFar = p;
            //}

            x = p.x;
            y = p.y;

            if (Mathf.Abs(x - xe) >= 1 || Mathf.Abs(y - ye) >= 1)
            {
                AStar(xs, ys, x, y, xe, ye, _myUnits);
            }
            else
            {
                PrintPath(xs, ys, xe, ye, _myUnits);
            }
        }
        else
        {
            // maybe we can use the uncomplete path? but how to find it?
            // we should have the best score so far saved somewhere
            // PrintPath(xs, ys, _bestSoFar.x, _bestSoFar.y, _myUnits); not working !
        }
    }

    private void CheckMoves(int x, int y, int xe, int ye, Unit[,] _myUnits, ref int xx, ref int yy)
    {
        // if the bottom unit isn't blocked it has to fall
        // no going up - just left and right, and drop
        
        // also, if on the ground, it can jump & go left and right
        foreach (Vector2Int m in _moves)
        {
            xx = x + m.x;
            yy = y + m.y;

            int unitFilled = _units[xx, yy] & 0x01;
            int underFilled = 0;

            if(yy - 1 > 0)
            {
                underFilled = _units[xx, yy - 1] & 0x01;

                if (m.y > -1 && yy - 1 > 0 && underFilled == 0)
                {
                    continue;
                }
            }


            if (xx > 0 && xx < _units.GetLength(0) && yy > 0 && yy < _units.GetLength(1) && (unitFilled == 0))
            {
                float score = _myUnits[x, y].scoreSoFar;

                score += 0.1f; // cost
                score += Mathf.Abs(xx - xe) + Mathf.Abs(yy - ye);

                bool visited = _myUnits[xx, yy].visited;

                if (_myUnits[xx, yy].scoreSoFar > score)
                {
                    _myUnits[xx, yy].visited = true;
                    _myUnits[xx, yy].scoreSoFar = score;
                    _myUnits[xx, yy].cameFromX = x;
                    _myUnits[xx, yy].cameFromY = y;
                }
                else if (!visited)
                {
                    _myUnits[xx, yy].visited = true;
                    _myUnits[xx, yy].scoreSoFar = score;
                    _myUnits[xx, yy].cameFromX = x;
                    _myUnits[xx, yy].cameFromY = y;
                }
                else
                {
                    continue;
                }

                _options.Add(score, new PathPoint(score, xx, yy));
            }
        }
    }

    private void PrintPath(int xs, int ys, int xe, int ye, Unit[,] _myUnits)
    {
        int x = xe, y = ye;
        while (x != xs || y != ys)
        {
            //for test purposes
            _units[x, y] |= 0x02;

            x = _myUnits[x, y].cameFromX;
            y = _myUnits[x, y].cameFromY;
        }
    }

    private Vector3 GetRelativeXY(Vector3 point, out int x, out int y)
    {
        point -= transform.position + offset + new Vector3(-size.x / 2, -size.y / 2);
        x = (int)Mathf.Ceil((point.x - _unitRadius) / _stepSize);
        y = (int)Mathf.Ceil((point.y - _unitRadius) / _stepSize);
        return point;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + offset, size);

        if (_stepSize > 0.05 && _isRunning)
        {
            int x = 0, y = 0;
            for (float xOffset = -size.x / 2; xOffset < size.x / 2; xOffset += _stepSize)
            {
                y = 0;
                for (float yOffset = -size.y / 2; yOffset < size.y / 2; yOffset += _stepSize)
                {
                    if ((_units[x, y] & 0x01) > 0)
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
        if (!_learned)
            Learn();

        _isRunning = true;
    }

    private void Update()
    {
        if (_isResting)
        {
            _time += Time.deltaTime;
            if (_time > _betweenUpdates)
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
