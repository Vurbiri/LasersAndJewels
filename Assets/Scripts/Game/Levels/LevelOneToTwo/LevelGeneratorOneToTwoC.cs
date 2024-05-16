using System.Collections;
using UnityEngine;

public class LevelGeneratorOneToTwoC : ALevelGeneratorTwoC
{
    public LevelGeneratorOneToTwoC(Vector2Int size, MonoBehaviour mono) : base(size, mono) { }

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
                waitResult.SetResult(new(_laserOne, _jewelsOne, _laserCurrent, _jewelsCurrent, _branchData));
            else
                waitResult.SetResult(null);
        }
        #endregion
    }
}
