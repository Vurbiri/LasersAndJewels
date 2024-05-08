using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ALevelGenerator
{
    protected Vector2Int _size;

    protected bool[,] _area;
    protected List<JewelSimple> _jewelsCurrent;
    protected LaserSimple _laserCurrent;
    protected int _typeCurrent, _countCurrent, _maxDistance = 8;
    protected Vector2Int _indexCurrent, _excluding;

    protected Func<bool> funcIsNotBetween;

    protected const int SHIFT_ERROR = 3;
    protected const int COUNT_ERROR = 35;

    public ALevelGenerator(Vector2Int size)
    {
        _size = size;
        funcIsNotBetween = IsNotBetweenOne;
    }

    protected void SetupOne(int count)
    {
        _countCurrent = count;
        _jewelsCurrent = new(count);

        _indexCurrent = URandom.Vector2Int(_size);
        _excluding = Direction2D.Random;

        Add();
        _laserCurrent = new(_indexCurrent, -_excluding, _typeCurrent);
        while (IsEmpty(_laserCurrent.Move()));
    }

    protected bool GenerateBase()
    {
        Vector2Int[] directions;
        bool result = false;
        int error = 0, count;
        while (_jewelsCurrent.Count < _countCurrent && error < COUNT_ERROR)
        {
            result = false;

            directions = Direction2D.Excluding(_excluding);
            foreach (byte index in URandom.ThreeIndexes)
            {
                if (result = TryAdd(directions[index]))
                    break;
            }

            if (!result)
            {
                error++;
                count = Mathf.Min((error >> SHIFT_ERROR) + 1, _jewelsCurrent.Count);
                for (int i = 0; i < count; i++)
                    RemoveLast();
            }
            else
            {
                Add();
            }

            if (_jewelsCurrent.Count < 2) return false;

            _excluding = _jewelsCurrent[^2] - _jewelsCurrent[^1];
        }

        return result;
    }

    protected bool TryAdd(Vector2Int direction)
    {
        _indexCurrent = _jewelsCurrent[^1].Index;
        Vector2Int start = _indexCurrent + direction;
        if (IsEmpty(start)) return false;

        Vector2Int end = start;
        int steps = _maxDistance;
        while (IsEmpty(end += direction) && --steps > 0) ;

        _indexCurrent = URandom.Vector2Int(start, end);
        if (funcIsNotBetween())
            return true;

        start = _indexCurrent;
        end = _jewelsCurrent[^1].Index;

        while ((_indexCurrent -= direction) != end)
            if (funcIsNotBetween())
                return true;

        _indexCurrent = start;

        while (IsEmpty(_indexCurrent += direction))
            if (funcIsNotBetween())
                return true;

        return false;
    }


    protected bool IsNotBetweenOne() => IsNotBetween(_laserCurrent, _jewelsCurrent);

    protected bool IsNotBetween(LaserSimple laser, List<JewelSimple> jewels)
    {
        Vector2Int a = laser.Index, b = jewels[0].Index;

        for (int i = 1; i < jewels.Count; i++)
        {
            if (_indexCurrent.IsBetween(a, b))
                return false;

            a = b; b = jewels[i].Index;
        }

        return !_indexCurrent.IsBetween(a, b);
    }

    protected virtual void Add()
    {
        _jewelsCurrent.Add(new(_indexCurrent, _typeCurrent));
        _area[_indexCurrent.x, _indexCurrent.y] = true;
    }

    protected void RemoveLast()
    {
        Vector2Int index = _jewelsCurrent.Pop().Index;
        _area[index.x, index.y] = false;
    }

    protected bool IsEmpty(Vector2Int index) => _area.IsCorrect(index) && !_area[index.x, index.y];
    protected bool IsCorrect(Vector2Int index) => _area.IsCorrect(index);
}
