using UnityEngine;

public class LevelOne : ALevel
{
    private readonly LevelGeneratorOne _generator;

    public override LevelType Type => LevelType.One;

    public LevelOne(Vector2Int size, ActorsPool actorsPool) : base(size, actorsPool)
    {
        _generator = new(size);
    }

    //public override void Initialize(Vector2Int size, ActorsPool actorsPool)
    //{
    //    base.Initialize(size, actorsPool);
    //    _generator = new(size);
    //}

    public override bool Create(int count, int maxDistance)
    {
        int type = Random.Range(0, 4);
        PositionsChainOne positionsChain = Generate();
        if (positionsChain == null) return false;

        _count = count;// + 1;
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

    //public override bool CheckChain()
    //{
    //    int count = 1;

    //    IJewel current = _jewels[0];
    //    Vector2Int index = current.Index, direction = current.Orientation, directionOld = _laserOne.Orientation;

    //    while (Visited(current) && !current.IsEnd)
    //    {
    //        while (IsEmpty(index += direction)) ;

    //        if (!IsCorrect(index)) break;

    //        current = this[index];
    //        directionOld = direction;
    //        direction = current.Orientation;
    //    }

    //    bool isLevelComplete = count == _count;

    //    if (!current.IsEnd && directionOld != -direction)
    //        _laserOne.PositionsRay[count++] = index.ToVector3();

    //    foreach (IJewel jewel in _jewels)
    //        jewel.Switch(isLevelComplete);

    //    _laserOne.SetRayPositions(count);

    //    return isLevelComplete;

    //    #region Local functions
    //    //======================
    //    bool Visited(IJewel jewel)
    //    {
    //        if (jewel.IsVisited) return false;

    //        _laserOne.PositionsRay[count++] = jewel.LocalPosition;
    //        return jewel.IsVisited = true;
    //    }
    //    #endregion
    //}
}
