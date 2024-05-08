using System.Collections.Generic;
using UnityEngine;

public abstract class ALevelGeneratorTwo : ALevelGenerator
{
    protected LaserSimple _laserOne;
    protected List<JewelSimple> _jewelsOne;
    protected int _chance, _typeBase;

    public ALevelGeneratorTwo(Vector2Int size) : base(size) { }

    protected bool SetupTwo(int count)
    {
        funcIsNotBetween = IsNotBetweenTwo;

        _laserOne = _laserCurrent;
        _jewelsOne = _jewelsCurrent;

        _countCurrent = count;
        _jewelsCurrent = new(count);

        Vector2Int[] directions = Direction2D.Line;
        Vector2Int laser;
        int error = 0;
        count = COUNT_ERROR >> 1;

        while (error++ < count)
        {
            _indexCurrent = URandom.Vector2Int(_size);
            if (!IsEmpty(_indexCurrent) && !IsNotBetween(_laserOne, _jewelsOne))
                continue;

            foreach (byte index in URandom.FourIndexes)
            {
                laser = _indexCurrent;
                _excluding = directions[index];
                while (IsEmpty(laser += _excluding)) ;
                if (!IsCorrect(laser) && laser != _laserOne.Index)
                {
                    _laserCurrent = new(laser, -_excluding, _typeCurrent);
                    Add();
                    return true;
                }
            }
        }
        return false;
    }

    protected bool IsNotBetweenTwo() => IsNotBetween(_laserCurrent, _jewelsCurrent) && IsNotBetween(_laserOne, _jewelsOne);

    protected override void Add()
    {
        _typeCurrent = URandom.IsTrue(_chance) || _jewelsCurrent.Count == 0 || _jewelsCurrent.Count == _countCurrent - 1 ? _typeBase : 0;
        base.Add();
    }
}
