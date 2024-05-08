using UnityEngine;

public class LevelTwo : ALevel
{
    private LevelGeneratorTwo _generator;

    private Laser _laserTwo;
    private IJewel _startOne, _startTwo;

    public override LevelType Type => LevelType.LevelTwo;

    protected const int SHIFT_RANDOM = 3;
    //protected const int TYPE_ONE = 1, TYPE_TWO = 2;

    public LevelTwo(Vector2Int size, ActorsPool actorsPool) : base(size, actorsPool)
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
        _count = count;
        int countOne = (count >> 1) - Random.Range(0, (count >> SHIFT_RANDOM) + 1);
        int countTwo = count - countOne;
        int typeOne = 1, typeTwo = 2;

        PositionsChainTwo positionsChain = Generate();
        if (positionsChain == null) return false;

        _jewels = new(count);

        Spawn(positionsChain.One, ref _laserOne, ref _startOne, typeOne);
        Spawn(positionsChain.Two, ref _laserTwo, ref _startTwo, typeTwo);

        return true;

        #region Local functions
        //======================
        PositionsChainTwo Generate()
        {
            PositionsChainTwo chain;
            int attempts = 0, maxAttempts = count << SHIFT_ATTEMPS;

            do chain = _generator.Generate(countOne, typeOne, countTwo, typeTwo, Random.Range(40, 61), maxDistance);
            while (++attempts < maxAttempts && chain == null);

            Debug.Log("attempts: " + attempts + "/" + maxAttempts + "\n============================");

            return chain;
        }
        //======================
        void Spawn(PositionsChainOne chain, ref Laser laser, ref IJewel start, int group)
        {
            laser = _actorsPool.GetLaser(chain.Laser, _count);
            Add(start = _actorsPool.GetJewel(chain.Jewels[0], 1, group));
            for (int i = 1; i < chain.Count; i++)
                Add(_actorsPool.GetJewel(chain.Jewels[i], i + 1, group));
            Add(_actorsPool.GetJewelEnd(chain.End));
        }
        #endregion
    }

    public override bool CheckChain()
    {
        bool isLevelComplete = CheckChain(_laserOne, _startOne, _laserTwo.IdType) + CheckChain(_laserTwo, _startTwo, _laserOne.IdType) == _count;

        foreach (IJewel jewel in _jewels)
            jewel.Switch(isLevelComplete);

        return isLevelComplete;
    }

    private int CheckChain(Laser laser, IJewel current, int errorType)
    {
        int count = 1;

        Vector2Int index = current.Index, direction = current.Orientation, directionOld = laser.Orientation;

        while (Visited(current) && !current.IsEnd)
        {
            while (IsEmpty(index += direction)) ;

            if (!IsCorrect(index)) break;

            current = this[index];
            directionOld = direction;
            direction = current.Orientation;
        }

        int countVisited = count - 1;

        if (current.IdType == errorType || (!current.IsEnd && directionOld != -direction))
            laser.PositionsRay[count++] = index.ToVector3();

        laser.SetRayPositions(count);

        return countVisited;

        #region Local functions
        //======================
        bool Visited(IJewel jewel)
        {
            if (jewel.IsVisited || jewel.IdType == errorType) return false;

            laser.PositionsRay[count++] = jewel.LocalPosition;
            return jewel.IsVisited = true;
        }
        #endregion
    }

    public override void Clear()
    {
        base.Clear();
        _laserTwo.Deactivate();
        _laserTwo = null;
    }
}
