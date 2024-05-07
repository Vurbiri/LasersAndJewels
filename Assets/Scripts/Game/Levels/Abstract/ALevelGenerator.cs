using System.Collections.Generic;
using UnityEngine;

public abstract class ALevelGenerator
{
    protected Vector2Int _size;

    protected bool[,] _area;
    protected List<JewelSimple> _jewels;
    protected LaserSimple _laser;
    protected int _type, _count, _maxDistance = 8;
    protected Vector2Int _current, _excluding;

    public ALevelGenerator(Vector2Int size)
    {
        _size = size;
    }

    protected void SetupOne(int count)
    {
        _count = count;
        _jewels = new(count);

        _current = URandom.Vector2Int(_size);
        _excluding = Direction2D.Random;

        Add();
        _laser = new(_current, -_excluding, _type);
        while (IsEmpty(_laser.Move()));
    }

    protected bool GenerateOne()
    {
        Vector2Int[] directions;
        bool result = false;
        int error = 0;
        while (_jewels.Count < _count && error < _count)
        {
            result = false;

            directions = Direction2D.Excluding(_excluding);
            foreach (byte index in URandom.ThreeIndexes)
            {
                if (result = TryAddOne(directions[index]))
                    break;
            }

            if (!result)
            {
                error++;
                for (int i = 0; i <= error >> 4; i++)
                    RemoveLast();
            }
            else
            {
                Add();
            }

            if (_jewels.Count < 2) return false;

            _excluding = _jewels[^2] - _jewels[^1];
        }

        Debug.Log(error);
        return result;
    }

    protected virtual bool TryAddOne(Vector2Int direction)
    {
        _current = _jewels[^1].Index;
        Vector2Int start = _current + direction;
        if (!IsEmpty(start)) return false;

        Vector2Int end = start;
        int steps = _maxDistance;
        while (IsEmpty(end += direction) && --steps > 0) ;

        if (IsNotBetween(_current = URandom.Vector2Int(start, end)))
            return true;

        start = _current;

        while (IsEmpty(_current -= direction))
            if (IsNotBetween(_current))
                return true;

        _current = start;

        while (IsEmpty(_current += direction))
            if (IsNotBetween(_current))
                return true;

        return false;
    }

    protected virtual bool IsNotBetween(Vector2Int position)
    {
        Vector2Int a = _laser.Index, b = _jewels[0].Index;

        for (int i = 1; i < _jewels.Count; i++)
        {
            if (position.IsBetween(a, b))
                return false;

            a = b; b = _jewels[i].Index;
        }

        return !position.IsBetween(a, b);
    }

    protected virtual void Add()
    {
        _jewels.Add(new(_current, _type));
        _area[_current.x, _current.y] = true;
    }

    protected virtual void RemoveLast()
    {
        Vector2Int index = _jewels.Pop().Index;
        _area[index.x, index.y] = false;
    }

    protected virtual bool IsEmpty(Vector2Int index) => _area.IsCorrect(index) && !_area[index.x, index.y];
    protected bool IsCorrect(Vector2Int index) => _area.IsCorrect(index);
}
