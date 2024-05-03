using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator
{
    private readonly BoolArea _area;
    private List<Vector2Int> _positions;
    private Vector2Int _zero;
    private int _maxDistance = 8;

    public LevelGenerator(Vector2Int size)
    {
        _area = new(size);
        _maxDistance = Mathf.Min(size.x, size.y);
    }

    public PositionsChain Simple(int count, int maxDistance)
    {
        _positions = new(count);
        _maxDistance = maxDistance;

        Vector2Int current = _zero = URandom.Vector2Int(_area.Size), excluding = Direction2D.Random;
        while (IsEmpty(_zero))
            _zero += excluding;

        Vector2Int zeroOrientation = -excluding;
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
                for (int i = 0; i <= Mathf.Min(error >> 3, _positions.Count - 2); i++)
                RemoveLast();
            }
            else
            {
                Add(current);
            }

            excluding = _positions[^2] - _positions[^1];
        }

        Debug.Log(error);
        Clear();
        return new(_positions, _zero, zeroOrientation, result);
    }

    private bool TryAdd(Vector2Int direction, out Vector2Int current)
    {
        current = _positions[^1];
        Vector2Int start = current + direction; 
        if(!IsEmpty(start)) return false;

        Vector2Int end = start;
        int steps = _maxDistance;
        while (IsEmpty(end += direction) && --steps > 0);

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
        _area.Add(position);
    }

    private void RemoveLast() => _area.Remove(_positions.Pop());

    public void Clear()
    {
        foreach (Vector2Int position in _positions)
            _area.Remove(position);
    }

    private bool IsEmpty(Vector2Int index) => _area.IsEmpty(index);


    #region Nested Classe
    //***********************************
    private class BoolArea
    {
        private readonly bool[,] _area;
        Vector2Int _size;

        public Vector2Int Size => _size;

        public BoolArea(Vector2Int size)
        {
            _area = new bool[size.x, size.y];
            _size = size;
        }

        public void Add(Vector2Int index) => _area[index.x, index.y] = true;
        public void Remove(Vector2Int index) => _area[index.x, index.y] = false;

        public bool IsEmpty(Vector2Int index) => index.x >= 0 && index.x < _size.x && index.y >= 0 && index.y < _size.y && !_area[index.x, index.y];
    }
    #endregion
}
