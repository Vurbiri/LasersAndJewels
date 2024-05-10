using System.Collections.Generic;
using UnityEngine;

public abstract class ALevelGeneratorTwo : ALevelGenerator
{
    protected LaserSimple _laserOne;
    protected List<Vector2Int> _jewelsOne;
    protected int _connectIndex;
    protected Vector2Int _orientationBranchIn, _orientationBranchOut, _orientationBranchNew;

    public ALevelGeneratorTwo(Vector2Int size) : base(size) { }

    protected virtual bool SetupTwo(int count)
    {
        if (!Branch()) return false;

        funcIsNotBetween = IsNotBetweenTwo;

        _laserOne = _laserCurrent;
        _jewelsOne = _jewelsCurrent;

        _countCurrent = count;
        _laserCurrent = new(_jewelsCurrent[_connectIndex], _orientationBranchOut);
        _jewelsCurrent = new(count);

        _orientationBranchNew = _excluding;
        Debug.Log(_orientationBranchIn + " == " + _orientationBranchNew);

        Add();

        return true;

        #region Local functions
        //======================
        bool Branch()
        {
            int minIndex = 2, maxIndex = _countCurrent - minIndex, leftIndex = (minIndex + maxIndex) >> 1, rightIndex = leftIndex;

            while (leftIndex >= minIndex || rightIndex < maxIndex)
            {
                if (leftIndex >= minIndex && TryInsert(_jewelsCurrent[_connectIndex = leftIndex--]))
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
                {
                    _excluding = direction;
                    return true;
                }
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
