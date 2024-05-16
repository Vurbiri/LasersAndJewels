using System.Collections;
using UnityEngine;

public class LevelGeneratorOneC : ALevelGeneratorC
{
    public LevelGeneratorOneC(Vector2Int size, MonoBehaviour mono) : base(size, mono) { }

    public WaitResult<PositionsChainOne> Generate_Wait(int count, int maxDistance)
    {
        WaitResult<PositionsChainOne> waitResult = new();
        _mono.StartCoroutine(Generate_Coroutine());
        return waitResult;

        #region Local function
        //=================================
        IEnumerator Generate_Coroutine()
        {
            WaitResult<bool> wait;
            yield return wait = GenerateBase_Wait(count, maxDistance);
            if (wait.Result)
                waitResult.SetResult(new(_laserCurrent, _jewelsCurrent));
            else
                waitResult.SetResult(null);
        }
        #endregion
    }
}
