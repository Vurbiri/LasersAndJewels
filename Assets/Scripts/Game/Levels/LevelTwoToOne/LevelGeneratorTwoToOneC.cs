using System.Collections;
using UnityEngine;

public class LevelGeneratorTwoToOneC : ALevelGeneratorTwoC
{
    public LevelGeneratorTwoToOneC(Vector2Int size, MonoBehaviour mono) : base(size, mono) { }

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

            yield return wait = SetupTwo_Wait(countTwo - 1);
            if (!wait.Result)
            {
                waitResult.SetResult(null);
                yield break;
            }
            yield return null;

            yield return wait = GenerateChain_Wait();
            if (!wait.Result)
            {
                waitResult.SetResult(null);
                yield break;
            }
            yield return null;

            while (_jewelsCurrent.Count > 2)
            {
                _excluding = _jewelsCurrent[^2] - _jewelsCurrent[^1];

                if (TryAdd() && SetupLaserTwo())
                {
                    Add();

                    _jewelsCurrent.Reverse();
                    waitResult.SetResult(new(_laserOne, _jewelsOne, _laserCurrent, _jewelsCurrent, _branchData));

                    yield break;
                }

                Debug.Log("SetupLaserTwo");

                RemoveLast();
                yield return null;
            }

            waitResult.SetResult(null);
        }
        #endregion
    }
}
