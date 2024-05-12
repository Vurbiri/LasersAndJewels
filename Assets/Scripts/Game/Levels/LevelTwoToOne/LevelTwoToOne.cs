using UnityEngine;

public class LevelTwoToOne : ALevelTwo
{
    private Laser _laserTwo;
    private JewelTwoToOne _jewelTwoToOne;
    protected override int StartFromRandom => 1;
    protected override int EndFromRandom => _count >> 2;

    public override LevelType Type => LevelType.TwoToOne;

    protected const int CHANCE_BASE = 45;

    public LevelTwoToOne(Vector2Int size, ActorsPool actorsPool) : base(size, actorsPool)
    {
        _generator = new LevelGeneratorTwoToOne(size);
    }

    public override bool Create(int count, int maxDistance)
    {
        _count = count;
        PositionsChainTwo positionsChain = Generate(maxDistance);
        if (positionsChain == null) return false;

        _count = positionsChain.One.Count + positionsChain.Two.Count + 2;
        _jewels = new(_count);

        int connect = positionsChain.Branch.Connect;
        PositionsChainOne jChain = positionsChain.One;
        _laserOne = _actorsPool.GetLaser(jChain.Laser, TYPE_ONE, _count);
        Spawn(jChain, 0, connect, TYPE_ONE, false, true);

        Add(_jewelTwoToOne = _actorsPool.GetJewelTwoToOne(positionsChain.Branch, TYPE_THREE, TYPE_ONE, TYPE_TWO, _count));
        Spawn(jChain, connect + 1, jChain.Count, TYPE_THREE, true);
        Add(_actorsPool.GetJewelEnd(jChain.End, TYPE_THREE));

        jChain = positionsChain.Two;
        _laserTwo = _actorsPool.GetLaser(jChain.Laser, TYPE_TWO, _count);
        Spawn(jChain, 0, jChain.Count , TYPE_TWO);
        Add(_actorsPool.GetJewel(jChain.End, 0, positionsChain.Two.Count + 1, TYPE_TWO));

        return true;

        #region Local function
        //======================
        void Spawn(PositionsChainOne chain, int start, int end, int type, bool zeroStart = false, bool zeroEnd = false)
        {
            int chance = CHANCE_BASE - (end - start);
            for (int i = start, k = 1; i < end; i++, k++)
                Add(_actorsPool.GetJewel(chain.Jewels[i], TypeLogic(i), k, type));

            //======================
            int TypeLogic(int i) => !(URandom.IsTrue(chance) || (i == start && !zeroStart)) || (i == start && zeroStart) || (i == end - 1 && zeroEnd) ? 0 : type;
        }
        #endregion
    }

    public override bool CheckChain()
    {
        int count = CheckChain(_laserOne) + CheckChain(_laserTwo) - 1;
        if (_jewelTwoToOne.IsVisited)
            count += CheckChain(_jewelTwoToOne);
        else
            _jewelTwoToOne.SetRayPositions(0);

        bool isLevelComplete = count == _count;
        foreach (IJewel jewel in _jewels)
            jewel.Switch(isLevelComplete);

        return isLevelComplete;
    }
    
    public override void Run()
    {
        _laserTwo.Run();
        base.Run();
    }

    public override void Clear()
    {
        base.Clear();
        _laserTwo.Deactivate();
        _laserTwo = null;
    }
}
