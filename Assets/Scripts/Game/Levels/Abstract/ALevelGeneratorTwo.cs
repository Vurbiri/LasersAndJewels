using System.Collections.Generic;
using UnityEngine;

public abstract class ALevelGeneratorTwo : ALevelGenerator
{
    protected LaserSimple _laserOne;
    protected List<Vector2Int> _jewelsOne;
    protected int _offset, _connectIndex;
    protected Vector2Int _orientationBranchIn, _orientationBranchOut;

    public ALevelGeneratorTwo(Vector2Int size) : base(size) { }

    protected virtual bool SetupTwo(int count)
    {
        if (!Branch()) return false;

        funcIsNotBetween = IsNotBetweenTwo;

        _laserOne = _laserCurrent;
        _jewelsOne = _jewelsCurrent;

        _countCurrent = count;
        _jewelsCurrent = new(count);

        Add();

        return true;

        #region Local functions
        //======================
        bool Branch()
        {
            int maxIndex = _countCurrent - _offset, leftIndex = Random.Range(_offset + 1, maxIndex), rightIndex = leftIndex;

            while (leftIndex >= _offset || rightIndex < maxIndex)
            {
                if (leftIndex >= _offset && TryInsert(_jewelsCurrent[_connectIndex = leftIndex--]))
                    return true;
                if (rightIndex < maxIndex && TryInsert(_jewelsCurrent[_connectIndex = rightIndex++]))
                    return true;
            }
            return false;
        }
        //======================
        bool TryInsert(Vector2Int connectCurrent)
        {
            _orientationBranchIn = _jewelsCurrent[_connectIndex - 1] - connectCurrent;
            _orientationBranchOut = _jewelsCurrent[_connectIndex + 1] - connectCurrent;

            foreach (Vector2Int direction in Direction2D.ExcludingRange(_orientationBranchIn, _orientationBranchOut))
            {
                if (CheckAdd(connectCurrent, direction))
                    return true;
            }

            return false;
        }
        #endregion
    }

    protected bool SetupLaserTwo()
    {
        Vector2Int laser;
        foreach (Vector2Int direction in Direction2D.LineRange)
        {
            laser = _indexCurrent;
            while (IsEmpty(laser += direction)) ;
            if (!IsCorrect(laser) && laser != _laserOne.Index)
            {
                _excluding = direction;
                _laserCurrent = new(laser, -direction);
                return true;
            }
        }

        return false;
    }

    protected bool IsNotBetweenTwo() => IsNotBetween(_laserCurrent, _jewelsCurrent) && IsNotBetween(_laserOne, _jewelsOne);
}
