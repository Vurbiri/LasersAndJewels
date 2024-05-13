using UnityEngine;

public class LevelOne : ALevel
{
    private readonly LevelGeneratorOne _generator;

    public override LevelType Type => LevelType.One;

    protected const int CHANCE_ZERO = 10;

    public LevelOne(Vector2Int size, ActorsPool actorsPool) : base(size, actorsPool)
    {
        _generator = new(size);
    }

    public override bool Create(int count, int maxDistance)
    {
        PositionsChainOne positionsChain = Generate();
        if (positionsChain == null) return false;

        int type = TYPE_ZERO;
        _colorGenerator.GenerateOne();

        _count = count;
        _jewels = new(count);

        _laserOne = _actorsPool.GetLaser(positionsChain.Laser, type, count + 1);

        count = 1;
        foreach (var jewel in positionsChain.Jewels)
            Add(_actorsPool.GetJewel(jewel, type, count++, type));

        Add(_actorsPool.GetJewelEnd(positionsChain.End, type));

        return true;

        #region Local functions
        //======================
        PositionsChainOne Generate()
        {
            PositionsChainOne chain;
            int attempts = 0, maxAttempts = count << SHIFT_ATTEMPS;

            do chain = _generator.Generate(count, maxDistance);
            while (++attempts < maxAttempts && chain == null);

            Debug.Log("attempts: " + attempts + "/" + maxAttempts + "\n============================");

            return chain;
        }
        #endregion
    }

    public override bool CheckChain()
    {
        bool isLevelComplete = CheckChain(_laserOne) == _count;

        foreach (IJewel jewel in _jewels)
            jewel.Switch(isLevelComplete);

        return isLevelComplete;
    }
}
