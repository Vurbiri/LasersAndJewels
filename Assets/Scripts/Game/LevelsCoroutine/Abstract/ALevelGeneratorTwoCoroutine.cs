using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ALevelGeneratorTwoCoroutine : ALevelGeneratorCoroutine
{
    protected LaserSimple _laserOne;
    protected List<Vector2Int> _jewelsOne;
    protected BranchData _branchData;

    public ALevelGeneratorTwoCoroutine(Vector2Int size, MonoBehaviour mono) : base(size, mono) { }

    public abstract PositionsChainTwo Generate(int countOne, int countTwo, int maxDistance);

    protected virtual bool SetupTwo(int count)
    {
        if (!Branch()) return false;

        funcIsNotBetween = IsNotBetweenTwo;

        _laserOne = _laserCurrent;
        _jewelsOne = _jewelsCurrent;

        _countCurrent = count;
        _laserCurrent = new(_jewelsCurrent[_branchData.Connect], Vector2Int.zero);
        _jewelsCurrent = new(count);

        Add();

        return true;

        #region Local functions
        //======================
        bool Branch()
        {
            Vector2Int branchIn, branchOut;
            int connectIndex;

            int minIndex = 2, maxIndex = _countCurrent - minIndex, leftIndex = (minIndex + maxIndex) >> 1, rightIndex = leftIndex;

            while (leftIndex >= minIndex || rightIndex < maxIndex)
            {
                if (leftIndex >= minIndex && TryInsert(_jewelsCurrent[connectIndex = leftIndex--]))
                    return true;
                if (rightIndex < maxIndex && TryInsert(_jewelsCurrent[connectIndex = rightIndex++]))
                    return true;
            }
            return false;

            #region Local functions
            //======================
            bool TryInsert(Vector2Int connectCurrent)
            {
                branchIn = _jewelsCurrent[connectIndex - 1] - connectCurrent;
                branchOut = _jewelsCurrent[connectIndex + 1] - connectCurrent;

                foreach (Vector2Int direction in Direction2D.ExcludingRange(branchIn, branchOut))
                {
                    if (CheckAdd(connectCurrent, direction))
                    {
                        _branchData = new(connectIndex, _jewelsCurrent[connectIndex], branchOut.NormalizeDirection(), direction);
                        return true;
                    }
                }

                return false;
            }
            #endregion
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
