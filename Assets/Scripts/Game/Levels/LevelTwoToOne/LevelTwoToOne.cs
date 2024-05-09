using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwoToOne : ALevel
{
    private readonly LevelGeneratorTwoToOne _generator;

    private Laser _laserTwo;
    private IJewel _startOne, _startTwo, _jewelTwoToOne;

    public override LevelType Type => LevelType.LevelTwoToOne;

    protected const int SHIFT_RANDOM = 4, CHANCE = 100;

    public LevelTwoToOne(Vector2Int size, ActorsPool actorsPool) : base(size, actorsPool)
    {
        _generator = new(size);
    }

    public override bool Create(int count, int maxDistance)
    {
        _count = count;
        int countTwo =  (count / 3) - Random.Range(0, (count >> SHIFT_RANDOM) + 1);
        int countOne = count - countTwo;
        

        PositionsChainTwo positionsChain = Generate();
        if (positionsChain == null) return false;

        _jewels = new(count);

        PositionsChainOne chain = positionsChain.One;

        _laserOne = _actorsPool.GetLaser(chain.Laser, TYPE_ONE, _count);
        Add(_startOne = _actorsPool.GetJewel(chain.Jewels[0], TYPE_ONE, 1, TYPE_ONE));
        Spawn(chain, 1, positionsChain.Connect, TYPE_ONE);

        Add(_actorsPool.GetJewelEnd(chain.Jewels[positionsChain.Connect], 0)); //****

        Spawn(chain, positionsChain.Connect + 1, chain.Count, TYPE_THREE, 1);
        Add(_actorsPool.GetJewelEnd(chain.End, TYPE_THREE));

        chain = positionsChain.Two;

        _laserTwo = _actorsPool.GetLaser(chain.Laser, TYPE_TWO, _count);
        Add(_startTwo = _actorsPool.GetJewel(chain.Jewels[0], TYPE_TWO, 1, TYPE_TWO));
        Spawn(chain, 1, chain.Count , TYPE_TWO);
        Add(_actorsPool.GetJewel(chain.End, URandom.IsTrue(CHANCE) ? TYPE_TWO : 0, positionsChain.Two.Count + 1, TYPE_TWO));


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
        void Spawn(PositionsChainOne chain, int start, int end, int type, int startNum = 2)
        {
            for (int i = start, k = startNum; i < end; i++, k++)
                Add(_actorsPool.GetJewel(chain.Jewels[i], URandom.IsTrue(CHANCE) ? type : 0, k, type));
            //Add(_actorsPool.GetJewelEnd(chain.End, type));
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

    public override void Run()
    {
        _laserOne.Run();
        _laserTwo.Run();
        _jewels.ForEach((j) => j.Run());
       // CheckChain();
    }

    public override void Clear()
    {
        base.Clear();
        _laserTwo.Deactivate();
        _laserTwo = null;
    }
}
