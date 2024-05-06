using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorTwo : ALevelGenerator<byte?>
{
    private List<JewelSimple> _jewelsOne;
    private LaserSimple _laserOne;
    private byte _chance, _currentType, _ignoreType;
    private int _count;

    public LevelGeneratorTwo(Vector2Int size) : base(size) { }

    public PositionsChainSimple[] Generate(int countOne, byte typeOne, int countTwo, byte typeTwo, byte chance, int maxDistance)
    {
        _area = new byte?[_size.x, _size.y];
        _maxDistance = maxDistance;
        _chance = chance;

        _currentType = typeOne;
        _count = countOne;
        if (!GenerateOne(countOne, typeOne))
            return null;

        _laserOne = _laser;
        _jewelsOne = _jewels;

        _currentType = typeTwo;
        _count = countTwo;
        return GenerateTwo(countTwo, typeTwo) ? new PositionsChainSimple []{ new(_laserOne, _jewelsOne), new(_laser, _jewels) } : null;
    }

    private bool GenerateTwo(int count, byte type)
    {
        _ignoreType = _type;
        _type = type;
        _jewels = new(count);

        Vector2Int current = Vector2Int.zero, laser = current, excluding = current;
        Vector2Int[] directions = Direction2D.Line;
        bool result = false;
        int error = -1;

        while (!result && error++ < count)
        {
            laser = current = URandom.Vector2Int(_size);
            if (!IsEmpty(current))
                continue;

            foreach (byte index in URandom.FourIndexes)
            {
                excluding = directions[index];
                while (IsMove(laser += excluding));
                if(result = (!IsCorrect(laser) && laser != _laser.Index)) break;
            }
        }

        if(!result) return false;

        _laser = new(laser, -excluding, type);
        Add(current);

        result = false;
        while (_jewels.Count < count && error < count)
        {
            result = false;

            directions = Direction2D.Excluding(excluding);
            foreach (byte index in URandom.ThreeIndexes)
                if (result = TryAddTwo(directions[index], out current))
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

    private void SetCurrentType() => _currentType = URandom.IsTrue(_chance) || _jewels.Count == 0 || _jewels.Count == _count - 1 ? _type : (byte)0;

    protected override bool TryAddOne(Vector2Int direction, out Vector2Int current)
    {
        SetCurrentType();
        return base.TryAddOne(direction, out current);
    }

    private bool TryAddTwo(Vector2Int direction, out Vector2Int current)
    {
        SetCurrentType();
        current = _jewels[^1].Index;
        Vector2Int start = current + direction;
        if (!IsMove(start)) return false;

        Vector2Int end = start;
        int steps = _maxDistance;
        while (IsMove(end += direction) && --steps > 0);

        if (IsNotBetweenTwo(current = URandom.Vector2Int(start, end)))
            return true;

        start = current;

        while (IsMove(current -= direction))
            if (IsNotBetweenTwo(current))
                return true;

        current = start;

        while (IsMove(current += direction))
            if (IsNotBetweenTwo(current))
                return true;

        return false;
    }

    protected virtual bool IsNotBetweenTwo(Vector2Int position)
    {
        if(_area[position.x, position.y] != null && !IsNotBetween(position)) return false;
        else if(_currentType == _type) return true;
        
        Vector2Int a = _laserOne.Index, b = _jewelsOne[0].Index;

        for (int i = 1; i < _jewelsOne.Count; i++)
        {
            if (position.IsBetween(a, b))
                return false;

            a = b; b = _jewelsOne[i].Index;
        }

        return true;
    }

    protected override void Add(Vector2Int index)
    {
        _jewels.Add(new(index, _currentType));
        _area[index.x, index.y] = _currentType;
    }

    protected override void RemoveLast()
    {
        Vector2Int index = _jewels.Pop().Index;
        _area[index.x, index.y] = null;
    }

    private bool IsMove(Vector2Int index)
    {
        byte? id;
        return IsCorrect(index) && ((id = _area[index.x, index.y]) == null || id == _ignoreType);
    }

    protected override bool IsEmpty(Vector2Int index) => IsCorrect(index) && _area[index.x, index.y] == null;
}
