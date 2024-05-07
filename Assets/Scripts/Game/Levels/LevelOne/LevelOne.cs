using UnityEngine;

public class LevelOne : ALevel
{
    private LevelGeneratorOne _generator;

    public override LevelType Type => LevelType.LevelOne;

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
        int type = Random.Range(0, 3);
        PositionsChainSimple positionsChain = Generate();
        if (positionsChain == null) return false;

        _count = count + 1;
        _jewels = new(count);

        _laserOne = _actorsPool.GetLaser(positionsChain.Laser, _count);

        count = 1;
        foreach (var jewel in positionsChain.Jewels)
            Add(_actorsPool.GetJewel(jewel, count++, type));

        Add(_actorsPool.GetJewelEnd(positionsChain.End));

        return true;

        #region Local functions
        //======================
        PositionsChainSimple Generate()
        {
            PositionsChainSimple chain;
            int attempts = 0, maxAttempts = count << 3;

            do chain = _generator.Generate(count, type, maxDistance);
            while (++attempts < maxAttempts && chain == null);

            Debug.Log("attempts: " + attempts + "/" + maxAttempts + "\n============================");

            return chain;
        }
        #endregion
    }

    public override bool CheckChain()
    {
        int count = 1;

        IJewel current = _jewels[0];
        Vector2Int index = current.Index, direction = current.Orientation, directionOld = _laserOne.Orientation;

        while (Visited(current) && direction != Vector2Int.zero)
        {
            while (IsEmpty(index += direction)) ;

            if (!IsCorrect(index)) break;

            current = this[index];
            directionOld = direction;
            direction = current.Orientation;
        }

        bool isLevelComplete = count == _count;

        if (direction != Vector2Int.zero && directionOld != -direction)
            _laserOne.PositionsRay[count++] = index.ToVector3();

        foreach (IJewel jewel in _jewels)
            jewel.Switch(isLevelComplete);

        _laserOne.SetRayPositions(count);

        return isLevelComplete;

        #region Local functions
        //======================
        bool Visited(IJewel jewel)
        {
            if (jewel.IsVisited) return false;

            _laserOne.PositionsRay[count++] = jewel.LocalPosition;
            return jewel.IsVisited = true;
        }
        #endregion
    }
}
