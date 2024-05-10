using UnityEngine;

public class LevelTwo : ALevel
{
    private readonly LevelGeneratorTwo _generator;

    private Laser _laserTwo;
    private IJewel _startOne, _startTwo;

    public override LevelType Type => LevelType.LevelTwo;

    protected const int SHIFT_RANDOM = 3, CHANCE = 50;

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

        PositionsChainTwo positionsChain = Generate();
        if (positionsChain == null) return false;

        _jewels = new(count);

        Spawn(positionsChain.One, ref _laserOne, ref _startOne, TYPE_ONE);
        Spawn(positionsChain.Two, ref _laserTwo, ref _startTwo, TYPE_TWO);

        return true;

        #region Local functions
        //======================
        PositionsChainTwo Generate()
        {
            PositionsChainTwo chain;
            int attempts = 0, maxAttempts = count << SHIFT_ATTEMPS;

            do chain = _generator.Generate(countOne, countTwo, maxDistance);
            while (++attempts < maxAttempts && chain == null);

            Debug.Log("attempts: " + attempts + "/" + maxAttempts + "\n============================");

            return chain;
        }
        //======================
        void Spawn(PositionsChainOne chain, ref Laser laser, ref IJewel start, int type)
        {
            laser = _actorsPool.GetLaser(chain.Laser, type, _count);
            Add(start = _actorsPool.GetJewel(chain.Jewels[0], type, 1, type));
            for (int i = 1; i < chain.Count; i++)
                Add(_actorsPool.GetJewel(chain.Jewels[i], URandom.IsTrue(CHANCE) ? type : 0, i + 1, type));
            Add(_actorsPool.GetJewelEnd(chain.End, type));
        }
        #endregion
    }

    public override void Run()
    {
        _laserTwo.Run();
        base.Run();
    }

    public override bool CheckChain()
    {
        bool isLevelComplete = CheckChain(_laserOne, _startOne, _laserTwo.LaserType) + CheckChain(_laserTwo, _startTwo, _laserOne.LaserType) == _count;

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
