using System.Collections.Generic;
using UnityEngine;

public abstract class ALevelGenerator<T>
{
    protected Vector2Int _size;

    protected T[,] _area;
    protected List<JewelSimple> _jewels;
    protected LaserSimple _laser;
    protected byte _type;
    protected int _maxDistance = 8;

    public ALevelGenerator(Vector2Int size)
    {
        _size = size;
    }

    protected bool GenerateOne(int count, byte type)
    {
        _type = type;
        _jewels = new(count);

        Vector2Int current = URandom.Vector2Int(_size), excluding = Direction2D.Random;

        Add(current);
        _laser = new(current, -excluding, type);
        while (IsEmpty(_laser.Move()));

        Vector2Int[] directions;
        bool result = false;
        int error = 0;
        while (_jewels.Count < count && error < count)
        {
            result = false;

            directions = Direction2D.Excluding(excluding);
            foreach (byte index in URandom.ThreeIndexes)
                if (result = TryAddOne(directions[index], out current))
                    break;

            if (!result)
            {
                error++;
                for (int i = 0; i <= Mathf.Min(error >> 4, _jewels.Count - 2); i++)
                    RemoveLast();
            }
            else
            {
                Add(current);
            }

            excluding = _jewels[^2] - _jewels[^1];
        }

        Debug.Log(error);
        return result;
    }

    protected virtual bool TryAddOne(Vector2Int direction, out Vector2Int current)
    {
        current = _jewels[^1].Index;
        Vector2Int start = current + direction;
        if (!IsEmpty(start)) return false;

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

        return true;
    }

    protected abstract void Add(Vector2Int index);
    protected abstract void RemoveLast();

    protected abstract bool IsEmpty(Vector2Int index);
    protected bool IsCorrect(Vector2Int index) => index.x >= 0 && index.x < _size.x && index.y >= 0 && index.y < _size.y;
}
