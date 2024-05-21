using System.Collections;
using UnityEngine;

public class LevelTwoToOne : ALevelTwo
{
    private Laser _laserTwo;
    private JewelTwoToOne _jewelTwoToOne;
    protected override int StartFromRandom => 1;
    protected override int EndFromRandom => _count >> 2;

    public override LevelType Type => LevelType.TwoToOne;

    public LevelTwoToOne(Vector2Int size, ActorsPool actorsPool) : base(size, actorsPool)
    {
        _generator = new LevelGeneratorTwoToOne(size);
        _generatorC = new LevelGeneratorTwoToOneC(size, actorsPool);
    }

    public override void Create()
    {
        _colorGenerator.GenerateThree();
        _count = _positionsChain.One.Count + _positionsChain.Two.Count + 2;
        _jewels = new(_count);

        int connect = _positionsChain.Branch.Connect;
        PositionsChainOne jChain = _positionsChain.One;
        _laserOne = _actorsPool.GetLaser(jChain.Laser, TYPE_ONE, _count);
        SpawnPartChain(jChain, 0, connect, TYPE_ONE, false, true);

        Add(_jewelTwoToOne = _actorsPool.GetJewelTwoToOne(_positionsChain.Branch, TYPE_THREE, TYPE_ONE, TYPE_TWO, _count));
        SpawnPartChain(jChain, connect + 1, jChain.Count, TYPE_THREE, true);
        Add(_actorsPool.GetJewelEnd(jChain.End, TYPE_THREE));

        jChain = _positionsChain.Two;
        _laserTwo = _actorsPool.GetLaser(jChain.Laser, TYPE_TWO, _count);
        SpawnPartChain(jChain, 0, jChain.Count, TYPE_TWO);
        Add(_actorsPool.GetJewel(jChain.End, 0, _positionsChain.Two.Count + 1, TYPE_TWO));
    }

    public override bool CheckChain()
    {
        int count = CheckChain(_laserOne) + CheckChain(_laserTwo) - 1;
        if (_jewelTwoToOne.IsVisited)
            count += CheckChain(_jewelTwoToOne);
        else
            _jewelTwoToOne.ResetRays();

        bool isLevelComplete = count == _count;
        foreach (IJewel jewel in _jewels)
            jewel.Switch(isLevelComplete);

        return isLevelComplete;
    }

    public override IEnumerator Run_Coroutine()
    {
        _laserTwo.Run();
        return base.Run_Coroutine();
    }

    public override void Reset()
    {
        base.Reset();

        if (_laserTwo != null)
        {
            _laserTwo.Deactivate();
            _laserTwo = null;
        }
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
