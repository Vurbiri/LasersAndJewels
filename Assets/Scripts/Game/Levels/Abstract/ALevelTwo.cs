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
}
