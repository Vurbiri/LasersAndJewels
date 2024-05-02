using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator
{
    private readonly bool[,] _values;
    private List<Vector2Int> _positions;
    private Vector2Int _size, _zero;
    private int _maxDistance = 8;

    public LevelGenerator(Vector2Int size)
    {
        _values = new bool[size.x, size.y];
        _maxDistance = Mathf.Min(size.x, size.y);
        _size = size;
    }

    public JewelsChain Simple(int count, int maxDistance)
    {
        _positions = new(count);
        _maxDistance = maxDistance;

        Vector2Int current = _zero = URandom.Vector2Int(_size), excluding = Direction2D.Random;
        while (IsEmpty(_zero))
            _zero += excluding;

        Vector2Int[] directions;
        bool result = false;
        int error = 0;
        Add(current);
        while (_positions.Count < count && error < count)
        {
            result = false;

            directions = Direction2D.Excluding(excluding);
            foreach (int index in URandom.ThreeIndexes)
                if (result = TryAdd(directions[index], out current))
                    break;

            if (!result)
            {
                error++;
                for (int i = 0; i <= Mathf.Min(error >> 2, _positions.Count - 3); i++)
                    RemoveLast();
            }
            else
            {
                Add(current);
            }

            excluding = _positions[^2] - _positions[^1];
        }

        //Debug.Log(attempt);
        Clear();
        return new(_positions, _zero, result);
    }

    private bool TryAdd(Vector2Int direction, out Vector2Int current)
    {
        current = _positions[^1];
        Vector2Int start = current + direction; 
        if(!IsEmpty(start)) return false;

        Vector2Int end = start;
        int steps = _maxDistance;
        while (IsEmpty(end += direction) && --steps > 0) ;

        if (IsNotBetween(current = URandom.Vector2Int(start, end)))
            return true;

        start = current;

        while (IsEmpty(current -= direction))
            if (IsNotBetween(current))
                return true;

        current = start;

        while (IsEmpty(current += direction))
            if (IsNotBetween(current))
                return true;

        return false;

        #region Local function
        //======================
        bool IsNotBetween(Vector2Int position)
        {
            Vector2Int a = _zero, b = _positions[0];

            for(int i = 1; i < _positions.Count; i++)
            {
                if (position.IsBetween(a, b))
                    return false;

                a = b; b = _positions[i];
            }

            return true;
        }
        #endregion
    }
        

    private void Add(Vector2Int position)
    {
        _positions.Add(position);
        _values[position.x, position.y] = true;
    }

    private void RemoveLast()
    {
        _values[_positions[^1].x, _positions[^1].y] = false;
        _positions.RemoveAt(_positions.Count - 1);
    }

    public void Clear()
    {
        foreach (Vector2Int index in _positions)
            _values[index.x, index.y] = false;
    }

    private bool IsEmpty(Vector2Int index) => IsCorrectIndex(index) && !_values[index.x, index.y];

    private bool IsCorrectIndex(Vector2Int index) => IsCorrectIndexX(index.x) && IsCorrectIndexY(index.y);
    private bool IsCorrectIndexX(int x) => x >= 0 && x < _size.x;
    private bool IsCorrectIndexY(int y) => y >= 0 && y < _size.y;
}
