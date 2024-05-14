using System.Collections;
using UnityEngine;

public class LevelOneCoroutine : ALevelCoroutine
{
    private readonly LevelGeneratorOneCoroutine _generator;
    PositionsChainOne _positionsChain;

    public override LevelType Type => LevelType.One;

    public LevelOneCoroutine(Vector2Int size, ActorsPool actorsPool) : base(size, actorsPool)
    {
        _generator = new(size, actorsPool);
    }

    public override WaitResult<bool> Generate_Wait(int count, int maxDistance)
    {
        WaitResult<bool> waitResult = new();

        _actorsPool.StartCoroutine(Generate_Coroutine());
        return waitResult;

        #region Local function
        //=================================
        IEnumerator Generate_Coroutine()
        {
            WaitResult<PositionsChainOne> chain;
            int attempts = 0, maxAttempts = count << SHIFT_ATTEMPS;

            do yield return chain = _generator.Generate_Wait(count, maxDistance);
            while (++attempts < maxAttempts && chain.Result == null);

            Debug.Log("attempts: " + attempts + "/" + maxAttempts + "\n============================");

            if ((_positionsChain = chain.Result) == null)
            {
                waitResult.SetResult(false);
                yield break;
            }

            _count = count;
            waitResult.SetResult(true);
        }
        #endregion
    }

    public override void Create()
    {
        _colorGenerator.GenerateOne();
        _jewels = new(_count);

        _laserOne = _actorsPool.GetLaser(_positionsChain.Laser, TYPE_ZERO, _count + 1);

        int count = 1;
        foreach (var jewel in _positionsChain.Jewels)
            Add(_actorsPool.GetJewel(jewel, TYPE_ZERO, count++, TYPE_ZERO));

        Add(_actorsPool.GetJewelEnd(_positionsChain.End, TYPE_ZERO));
    }

    public override bool CheckChain()
    {
        bool isLevelComplete = CheckChain(_laserOne) == _count;

        foreach (IJewel jewel in _jewels)
            jewel.Switch(isLevelComplete);

        return isLevelComplete;
    }
}
