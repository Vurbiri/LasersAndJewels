using System.Collections;
using UnityEngine;

public class LevelGeneratorTwoC : ALevelGeneratorTwoC
{
    public LevelGeneratorTwoC(Vector2Int size, MonoBehaviour mono) : base(size, mono) { }

    public override WaitResult<PositionsChainTwo> Generate_Wait(int countOne, int countTwo, int maxDistance)
    {
        WaitResult<PositionsChainTwo> waitResult = new();
        _mono.StartCoroutine(Generate_Coroutine());
        return waitResult;

        #region Local: Generate_Coroutine()
        //=================================
        IEnumerator Generate_Coroutine()
        {
            WaitResult<bool> wait;

            yield return wait = GenerateBase_Wait(countOne, maxDistance);
            if (!wait.Result)
            {
                waitResult.SetResult(null);
                yield break;
            }
            yield return null;

            yield return wait = SetupTwo_Wait(countTwo);
            if (!wait.Result)
            {
                waitResult.SetResult(null);
                yield break;
            }
            yield return null;

            yield return wait = GenerateChain_Wait();
            if (wait.Result)
                waitResult.SetResult(new(_laserOne, _jewelsOne, _laserCurrent, _jewelsCurrent));
            else
                waitResult.SetResult(null);

        }
        #endregion
    }

    protected new WaitResult<bool> SetupTwo_Wait(int count)
    {
        funcIsNotBetween = IsNotBetweenTwo;

        _laserOne = _laserCurrent;
        _jewelsOne = _jewelsCurrent;

        _countCurrent = count;
        _jewelsCurrent = new(count);

        int error = 0;
        count = COUNT_ERROR >> 1;

        while (error++ < count)
        {
            if (IsEmpty(_indexCurrent = URandom.Vector2Int(_size)) && IsNotBetween(_laserOne, _jewelsOne) && SetupLaserTwo())
            {
                Add();
                return new(true);
            }
        }
        return new(false);
    }
}
