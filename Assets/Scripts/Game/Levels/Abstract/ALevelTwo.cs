using UnityEngine;

public abstract class ALevelTwo : ALevel
{
    protected ALevelGeneratorTwo _generator;

    protected abstract int StartFromRandom { get; }
    protected abstract int EndFromRandom { get; }

    public ALevelTwo(Vector2Int size, ActorsPool actorsPool) : base(size, actorsPool) { }

    protected PositionsChainTwo Generate(int maxDistance)
    {
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

        Debug.Log("attempts: " + attempts + "/" + maxAttempts + "\n============================");

        return chain;
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
