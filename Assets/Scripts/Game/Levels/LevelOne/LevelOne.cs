using System.Collections;
using UnityEngine;

public class LevelOne : ALevel
{
    private readonly LevelGeneratorOne _generator;
    private readonly LevelGeneratorOneC _generatorC;
    private PositionsChainOne _positionsChain;

    public override LevelType Type => LevelType.One;

    public LevelOne(Vector2Int size, ActorsPool actorsPool) : base(size, actorsPool)
    {
        _generator = new(size);
        _generatorC = new(size, actorsPool);
    }

    public override bool Generate(int count, int maxDistance)
    {
        _count = count;

        PositionsChainOne chain;
        int attempts = 0, maxAttempts = count << SHIFT_ATTEMPS;

        do chain = _generator.Generate(count, maxDistance);
        while (++attempts < maxAttempts && chain == null);

        Debug.Log($"{Type} count: {_count}. attempts: {attempts} / {maxAttempts} \n====================================");

        return (_positionsChain = chain) != null;
    }
    public override WaitResult<bool> Generate_Wait(int count, int maxDistance)
    {
        WaitResult<bool> waitResult = new();
        _count = count;
        _isGeneration = true;
        _actorsPool.StartCoroutine(Generate_Coroutine());
        return waitResult;

        #region Local: Generate_Coroutine()
        //=================================
        IEnumerator Generate_Coroutine()
        {
            WaitResult<PositionsChainOne> chain;
            int attempts = 0, maxAttempts = count << SHIFT_ATTEMPS;

            do yield return chain = _generatorC.Generate_Wait(count, maxDistance);
            while (_isGeneration && ++attempts < maxAttempts && chain.Result == null);

            Debug.Log($"{Type} count: {_count}. attempts: {attempts} / {maxAttempts} \n====================================");

            _isGeneration = false;
            waitResult.SetResult((_positionsChain = chain.Result) != null);
        }
        #endregion
    }

    public override void Create()
    {
        _colorGenerator.GenerateOne();
        _jewels = new(_count);

        _laserOne = _actorsPool.GetLaser(_positionsChain.Laser, TYPE_ONE, _count + 1);

        int count = 1;
        foreach (var jewel in _positionsChain.Jewels)
            Add(_actorsPool.GetJewel(jewel, TYPE_ONE, count++, TYPE_ONE));

        Add(_actorsPool.GetJewelEnd(_positionsChain.End, TYPE_ONE));
    }

    public override bool CheckChain()
    {
        bool isLevelComplete = CheckChain(_laserOne) == _count;

        foreach (IJewel jewel in _jewels)
            jewel.Switch(isLevelComplete);

        return isLevelComplete;
    }
}
