using UnityEngine;

public class LevelOneToTwo : ALevelTwo
{
    private JewelOneToTwo _jewelOneToTwo;
    protected override int StartFromRandom => 1;
    protected override int EndFromRandom => _count >> 2;

    public override LevelType Type => LevelType.OneToTwo;

    protected const int CHANCE_BASE = 45;

    public LevelOneToTwo(Vector2Int size, ActorsPool actorsPool) : base(size, actorsPool)
    {
        _generator = new LevelGeneratorOneToTwo(size);
    }

    public override bool Create(int count, int maxDistance)
    {
        _count = count;
        PositionsChainTwo positionsChain = Generate(maxDistance);
        if (positionsChain == null) return false;

        _jewels = new(_count--);

        int connect = positionsChain.Branch.Connect;
        PositionsChainOne jChain = positionsChain.One;
        _laserOne = _actorsPool.GetLaser(jChain.Laser, TYPE_THREE, _count);
        Spawn(jChain, 0, connect, TYPE_THREE, false, true);

        Add(_jewelOneToTwo = _actorsPool.GetJewelOneToTwo(positionsChain.Branch, TYPE_ONE, TYPE_TWO, TYPE_THREE, _count));
        
        Spawn(jChain, connect + 1, jChain.Count, TYPE_ONE, true);
        Add(_actorsPool.GetJewelEnd(jChain.End, TYPE_ONE));

        jChain = positionsChain.Two;
        Spawn(jChain, 0, jChain.Count, TYPE_TWO, true);
        Add(_actorsPool.GetJewelEnd(jChain.End, TYPE_TWO));

        return true;

        #region Local function
        //======================
        void Spawn(PositionsChainOne chain, int start, int end, int type, bool zeroStart = false, bool zeroEnd = false)
        {
            int chance = CHANCE_BASE - (end - start);
            for (int i = start, k = 1; i < end; i++, k++)
                Add(_actorsPool.GetJewel(chain.Jewels[i], TypeLogic(i), k, type));

            int TypeLogic(int i) => !(URandom.IsTrue(chance) || (i == start && !zeroStart)) || (i == start && zeroStart) || (i == end - 1 && zeroEnd) ? 0 : type;
        }
        #endregion
    }

    public override bool CheckChain()
    {
        int count = CheckChain(_laserOne) - 1;
        if (_jewelOneToTwo.IsVisited)
            count += CheckChain(_jewelOneToTwo.LaserOne) + CheckChain(_jewelOneToTwo.LaserTwo);
        else
            _jewelOneToTwo.ResetRays();

        bool isLevelComplete = count == _count;
        foreach (IJewel jewel in _jewels)
            jewel.Switch(isLevelComplete);

        return isLevelComplete;
    }
}
