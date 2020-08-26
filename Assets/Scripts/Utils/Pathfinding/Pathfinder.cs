using DG.Tweening.Plugins.Core.PathCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct PathPoint
{
    public int score, x, y;

    public PathPoint(int score, int x, int y)
    {
        this.score = score;
        this.x = x;
        this.y = y;
    }
}

public class IntComparer : IComparer<int>
{
    public int Compare(int i1, int i2)
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
    private bool _isResting;

    [SerializeField]
    private Vector2Int[] _moves;

    private SortedList<int, PathPoint> _options;
    private List<Vector2> _path;

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

        GetRelativeXY(start, out xStart, out yStart);
        GetRelativeXY(end, out xEnd, out yEnd);

        // convert if elses to calculations if needed optimization
        // convert to if else for optimazation
        try
        {
            _units[xStart, yStart] = (byte)(0x02 | _units[xStart, yStart]);
            _units[xEnd, yEnd] = (byte)(0x02 | _units[xEnd, yEnd]);

            _path = new List<Vector2>();
            _options = new SortedList<int, PathPoint>(new IntComparer());
            AStar(xStart, yStart, xStart, yStart, xEnd, yEnd);
        }
        catch { }
    }

    private void AStar(int xs, int ys, int x, int y, int xe, int ye)
    {
        _path.Add(
            new Vector2(xs, ys)
        );

        int xx = 0, yy = 0;

        foreach(Vector2Int m in _moves)
        {
            xx = x + m.x;
            yy = y + m.y;

            if(xx > 0 && xx < _units.GetLength(0) && yy > 0 && yy < _units.GetLength(1))
            {
                int score = 0;

                score += Mathf.Abs(xx - xs);
                score += Mathf.Abs(yy - ys);

                score += Mathf.Abs(xx - xe);
                score += Mathf.Abs(yy - ye);

                _options.Add(score, new PathPoint(score, xx, yy));
            }
        }

        try
        {
            PathPoint p = _options.First().Value;
            _options.RemoveAt(0);
            x = p.x;
            y = p.y;

            // stack overflow here
            if (Mathf.Abs(x - xe) > 1 || Mathf.Abs(y - ye) > 1)
            {
                AStar(xs, ys, x, y, xe, ye);
            }
            else
            {
                foreach(Vector2 v in _path)
                {
                    _units[(int)v.x, (int)v.y] = 0x02;
                }
            }
        }
        catch(System.Exception e) {
            print(e.ToString());
        }
    }

    private Vector3 GetRelativeXY(Vector3 point, out int x, out int y)
    {
        point -= transform.position + offset + new Vector3(-size.x / 2, -size.y / 2);
        x = (int)Mathf.Ceil((point.x - _unitRadius) / _stepSize);
        y = (int)Mathf.Ceil((point.y - _unitRadius) / _stepSize);
        return point;
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
