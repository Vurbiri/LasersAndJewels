using System.Collections;
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
        _generatorC = new LevelGeneratorTwoC(size, actorsPool);
    }

    public override void Create()
    {
        _colorGenerator.GenerateTwo();
        _jewels = new(_count);

        Spawn(_positionsChain.One, ref _laserOne, TYPE_ONE);
        Spawn(_positionsChain.Two, ref _laserTwo, TYPE_TWO);

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

    public override IEnumerator Run_Coroutine()
    {
        _laserTwo.Run();
        return base.Run_Coroutine();
    }

    public override bool CheckChain()
    {
        bool isLevelComplete = CheckChain(_laserOne) + CheckChain(_laserTwo) == _count;

        foreach (IJewel jewel in _jewels)
            jewel.Switch(isLevelComplete);

        return isLevelComplete;
    }

    public override IEnumerator Clear_Coroutine()
    {
        WaitAll waitAll = new(_actorsPool);
        waitAll.Add(base.Clear_Coroutine());
        waitAll.Add(_laserTwo.Deactivate_Coroutine());
        yield return waitAll;
        _laserTwo = null;
    }
}
