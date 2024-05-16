using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ALevelGeneratorTwoC : ALevelGeneratorC
{
    protected LaserSimple _laserOne;
    protected List<Vector2Int> _jewelsOne;
    protected BranchData _branchData;

    public ALevelGeneratorTwoC(Vector2Int size, MonoBehaviour mono) : base(size, mono) { }

    public abstract WaitResult<PositionsChainTwo> Generate_Wait(int countOne, int countTwo, int maxDistance);

    protected virtual WaitResult<bool> SetupTwo_Wait(int count)
    {
        WaitResult<bool> waitResult = new();
        _mono.StartCoroutine(SetupTwo_Coroutine());
        return waitResult;

        #region Local: SetupTwo_Coroutine(), Branch_Coroutine()
        IEnumerator SetupTwo_Coroutine()
        {
            yield return _mono.StartCoroutine(Branch_Coroutine());
            if (!waitResult.keepWaiting)
                yield break;

            funcIsNotBetween = IsNotBetweenTwo;

            _laserOne = _laserCurrent;
            _jewelsOne = _jewelsCurrent;

            _countCurrent = count;
            _laserCurrent = new(_jewelsCurrent[_branchData.Connect], Vector2Int.zero);
            _jewelsCurrent = new(count);

            Add();

            waitResult.SetResult(true);
        }
        //======================
        IEnumerator Branch_Coroutine()
        {
            Vector2Int branchIn, branchOut;
            int connectIndex, minIndex = 2, maxIndex = _countCurrent - minIndex, leftIndex = (minIndex + maxIndex) >> 1, rightIndex = leftIndex;

            while (leftIndex >= minIndex || rightIndex < maxIndex)
            {
                if (leftIndex >= minIndex && TryInsert(_jewelsCurrent[connectIndex = leftIndex--]))
                    yield break;
                if (rightIndex < maxIndex && TryInsert(_jewelsCurrent[connectIndex = rightIndex++]))
                    yield break;

                yield return null;
            }

            waitResult.SetResult(false);

            #region Local: TryInsert(...)
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
