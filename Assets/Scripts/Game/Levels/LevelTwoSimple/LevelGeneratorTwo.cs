using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorTwo : ALevelGenerator
{
    private LaserSimple _laserOne;
    private List<JewelSimple> _jewelsOne;
    private int _chance, _currentType;

    private Func<Vector2Int, bool> funcIsNotBetween;

    public LevelGeneratorTwo(Vector2Int size) : base(size) { }

    public PositionsChainSimple[] Generate(int countOne, int typeOne, int countTwo, int typeTwo, int chance, int maxDistance)
    {
        _area = new bool[_size.x, _size.y];
        _maxDistance = maxDistance;
        _chance = chance;

        _currentType = _type = typeOne;
        funcIsNotBetween = base.IsNotBetween;
        SetupOne(countOne);
        if (!GenerateOne())
            return null;

        _laserOne = _laser;
        _jewelsOne = _jewels;

        _currentType = _type = typeTwo;
        funcIsNotBetween = IsNotBetweenTwo;
        return SetupTwo(countTwo) && GenerateOne() ? new PositionsChainSimple []{ new(_laserOne, _jewelsOne), new(_laser, _jewels) } : null;
    }

    private bool SetupTwo(int count)
    {
        _count = count;
        _jewels = new(count);

        Vector2Int[] directions = Direction2D.Line;
        Vector2Int laser = Vector2Int.zero;
        bool result = false;
        int error = 0;
        count >>= 3;

        while (!result && error++ < count)
        {
            laser = _current = URandom.Vector2Int(_size);
            if (!IsEmpty(_current))
                continue;

            foreach (byte index in URandom.FourIndexes)
            {
                _excluding = directions[index];
                while (IsEmpty(laser += _excluding)) ;
                if (result = (!IsCorrect(laser) && laser != _laser.Index)) break;
            }
        }

        _laser = new(laser, -_excluding, _type);
        Add();

        return result;
    }

    protected override bool IsNotBetween(Vector2Int position) => funcIsNotBetween(position);

    private bool IsNotBetweenTwo(Vector2Int position)
    {
        Vector2Int a = _laserOne.Index, b = _jewelsOne[0].Index;

        for (int i = 1; i < _jewelsOne.Count; i++)
        {
            if (position.IsBetween(a, b))
                return false;

            a = b; b = _jewelsOne[i].Index;
        }

        return !position.IsBetween(a, b) && base.IsNotBetween(position);
    }

    protected override void Add()
    {
        _type = URandom.IsTrue(_chance) || _jewels.Count == 0 || _jewels.Count == _count - 1 ? _currentType : 0;
        base.Add();
    }
}
