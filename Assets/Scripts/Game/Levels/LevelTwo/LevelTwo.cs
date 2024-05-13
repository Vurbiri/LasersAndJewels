using UnityEngine;

public class LevelTwo : ALevelTwo
{
    private Laser _laserTwo;

    protected override int StartFromRandom => 0;
    protected override int EndFromRandom => _count >> 3;

    public override LevelType Type => LevelType.Two;

    public LevelTwo(Vector2Int size, ActorsPool actorsPool) : base(size, actorsPool)
    {
        _generator = new LevelGeneratorTwo(size);
    }

    public override bool Create(int count, int maxDistance)
    {
        _count = count;

        PositionsChainTwo positionsChain = Generate(maxDistance);
        if (positionsChain == null) return false;

        _colorGenerator.GenerateTwo();
        _jewels = new(count);

        Spawn(positionsChain.One, ref _laserOne, TYPE_ONE);
        Spawn(positionsChain.Two, ref _laserTwo, TYPE_TWO);

        return true;

        #region Local function
        //======================
        void Spawn(PositionsChainOne chain, ref Laser laser, int type)
        {
            int chance = CHANCE_BASE - chain.Count;
            laser = _actorsPool.GetLaser(chain.Laser, type, _count);
            for (int i = 0; i < chain.Count; i++)
                Add(_actorsPool.GetJewel(chain.Jewels[i], URandom.IsTrue(chance) || i == 0 ? type : 0, i + 1, type));
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
        bool isLevelComplete = CheckChain(_laserOne) + CheckChain(_laserTwo) == _count;

        foreach (IJewel jewel in _jewels)
            jewel.Switch(isLevelComplete);

        return isLevelComplete;
    }

    public override void Clear()
    {
        base.Clear();
        _laserTwo.Deactivate();
        _laserTwo = null;
    }
}
