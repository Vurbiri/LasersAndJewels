using UnityEngine;

public class LevelTwoToOne : ALevel
{
    private readonly LevelGeneratorTwoToOne _generator;

    private Laser _laserTwo;
    private JewelTwoToOne _jewelTwoToOne;

    public override LevelType Type => LevelType.LevelTwoToOne;

    protected const int SHIFT_RANDOM = 2, CHANCE = 100;

    public LevelTwoToOne(Vector2Int size, ActorsPool actorsPool) : base(size, actorsPool)
    {
        _generator = new(size);
    }

    public override bool Create(int count, int maxDistance)
    {
        _count = count;
        
        PositionsChainTwo positionsChain = Generate();
        if (positionsChain == null) return false;

        _jewels = new(count);

        PositionsChainOne chain = positionsChain.One;

        _laserOne = _actorsPool.GetLaser(chain.Laser, TYPE_ONE, _count);
        Spawn(chain, 0, positionsChain.Connect, TYPE_ONE);

        Add(_jewelTwoToOne = _actorsPool.GetJewelTwoToOne(chain.Jewels[positionsChain.Connect], TYPE_THREE, _count));
        Spawn(chain, positionsChain.Connect + 1, chain.Count, TYPE_THREE);
        Add(_actorsPool.GetJewelEnd(chain.End, TYPE_THREE));

        chain = positionsChain.Two;

        _laserTwo = _actorsPool.GetLaser(chain.Laser, TYPE_TWO, _count);
        Spawn(chain, 0, chain.Count , TYPE_TWO);
        Add(_actorsPool.GetJewel(chain.End, URandom.IsTrue(CHANCE) ? TYPE_TWO : 0, positionsChain.Two.Count + 1, TYPE_TWO));


        return true;

        #region Local functions
        //======================
        PositionsChainTwo Generate()
        {
            PositionsChainTwo chain;
            int attempts = 0, maxAttempts = count << SHIFT_ATTEMPS;
            int countOne, countTwo;

            do
            {
                countTwo = (count >> 1) - Random.Range(1, count >> SHIFT_RANDOM);
                countOne = count - countTwo;

                chain = _generator.Generate(countOne, countTwo, maxDistance);
            }
            while (++attempts < maxAttempts && chain == null);

            Debug.Log("attempts: " + attempts + "/" + maxAttempts + "\n============================");

            return chain;
        }
        //======================
        void Spawn(PositionsChainOne chain, int start, int end, int type)
        {
            for (int i = start, k = 1; i < end; i++, k++)
                Add(_actorsPool.GetJewel(chain.Jewels[i], URandom.IsTrue(CHANCE) || k == 1 ? type : 0, k, type));
        }
        #endregion
    }

    public override bool CheckChain()
    {
        int count = CheckChain(_laserOne) + CheckChain(_laserTwo);
        if (_jewelTwoToOne.IsVisited)
            count += CheckChain(_jewelTwoToOne) - 1;
        else
            _jewelTwoToOne.SetRayPositions(0);


        bool isLevelComplete = count == _count;
        foreach (IJewel jewel in _jewels)
            jewel.Switch(isLevelComplete);

        return isLevelComplete;
    }

    private int CheckChain(ILaser laser)
    {
        int count = 1, currentType = laser.LaserType;

        Vector2Int direction = laser.Orientation, index = laser.Index, directionOld = -direction;
        IJewel current = null;

        do
        {
            while (IsEmpty(index += direction)) ;

            if (!IsCorrect(index)) break;

            current = this[index];
            directionOld = direction;
            direction = current.Orientation;
        }
        while (Visited(current) && !current.IsEnd);

        int countVisited = count - 1;

        if (current != null && (!IsType(current.IdType) || (!current.IsEnd && directionOld != -direction)))
            laser.PositionsRay[count++] = index.ToVector3();

        laser.SetRayPositions(count);

        return countVisited;

        #region Local functions
        //======================
        bool Visited(IJewel jewel)
        {
            if (jewel.IsVisited || !IsType(jewel.IdType)) return false;

            laser.PositionsRay[count++] = jewel.LocalPosition;
            return jewel.IsVisited = true;
        }
        bool IsType(int type) => type == 0 || type == currentType;

        #endregion
    }

    public override void Run()
    {
        _laserOne.Run();
        _laserTwo.Run();
        _jewels.ForEach((j) => j.Run());
        CheckChain();
    }

    public override void Clear()
    {
        base.Clear();
        _laserTwo.Deactivate();
        _laserTwo = null;
        _jewelTwoToOne.Deactivate();
        _jewelTwoToOne = null;
    }
}
