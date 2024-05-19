using System.Collections;
using UnityEngine;

public abstract class ALevelTwo : ALevel
{
    protected ALevelGeneratorTwo _generator;
    protected ALevelGeneratorTwoC _generatorC;
    protected PositionsChainTwo _positionsChain;

    protected abstract int StartFromRandom { get; }
    protected abstract int EndFromRandom { get; }
    protected override int RemoveCountHint => 2;

    public ALevelTwo(Vector2Int size, ActorsPool actorsPool) : base(size, actorsPool) { }

    public override bool Generate(int count, int maxDistance)
    {
        _count = count;

        PositionsChainTwo chain;
        int attempts = 0, maxAttempts = _count << SHIFT_ATTEMPS;
        int countOne, countTwo;

        do
        {
            countTwo = (_count >> 1) - Random.Range(StartFromRandom, EndFromRandom);
            countOne = _count - countTwo;

            chain = _generator.Generate(countOne, countTwo, maxDistance);
        }
        while (++attempts < maxAttempts && chain == null);

        Debug.Log($"{Type} count: {_count}. attempts: {attempts} / {maxAttempts} \n====================================");

        _positionsChain = chain;
        return (_positionsChain = chain) != null;
    }

    public override WaitResult<bool> Generate_Wait(int count, int maxDistance)
    {
        WaitResult<bool> waitResult = new();
        _count = count;
        _isGeneration = true;
        _actorsPool.StartCoroutine(Generate_Coroutine());
        return waitResult;

        #region Local function
        //=================================
        IEnumerator Generate_Coroutine()
        {
            WaitResult<PositionsChainTwo> chain;
            int attempts = 0, maxAttempts = _count << SHIFT_ATTEMPS;
            int countOne, countTwo;

            do
            {
                countTwo = (_count >> 1) - Random.Range(StartFromRandom, EndFromRandom);
                countOne = _count - countTwo;

                yield return chain = _generatorC.Generate_Wait(countOne, countTwo, maxDistance);
            }
            while (_isGeneration && ++attempts < maxAttempts && chain.Result == null);

            Debug.Log($"{Type} count: {_count}. attempts: {attempts} / {maxAttempts} \n====================================");

            _isGeneration = false;
            waitResult.SetResult((_positionsChain = chain.Result) != null);
        }
        #endregion
    }


    protected void SpawnPartChain(PositionsChainOne chain, int start, int end, int type, bool zeroStart = false, bool zeroEnd = false)
    {
        int chance = CHANCE_BASE - (end - start);
        for (int i = start, k = 1; i < end; i++, k++)
            Add(_actorsPool.GetJewel(chain.Jewels[i], TypeLogic(i), k, type));

        //=== Local function ===================
        int TypeLogic(int i) => !(URandom.IsTrue(chance) || (i == start && !zeroStart)) || (i == start && zeroStart) || (i == end - 1 && zeroEnd) ? 0 : type;
    }
}
