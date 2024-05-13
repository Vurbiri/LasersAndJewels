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
    }

    public override bool Create(int count, int maxDistance)
    {
        _count = count;
        PositionsChainTwo positionsChain = Generate(maxDistance);
        if (positionsChain == null) return false;

        _colorGenerator.GenerateThree();
        _count = positionsChain.One.Count + positionsChain.Two.Count + 2;
        _jewels = new(_count);

        int connect = positionsChain.Branch.Connect;
        PositionsChainOne jChain = positionsChain.One;
        _laserOne = _actorsPool.GetLaser(jChain.Laser, TYPE_ONE, _count);
        SpawnPartChain(jChain, 0, connect, TYPE_ONE, false, true);

        Add(_jewelTwoToOne = _actorsPool.GetJewelTwoToOne(positionsChain.Branch, TYPE_THREE, TYPE_ONE, TYPE_TWO, _count));
        SpawnPartChain(jChain, connect + 1, jChain.Count, TYPE_THREE, true);
        Add(_actorsPool.GetJewelEnd(jChain.End, TYPE_THREE));

        jChain = positionsChain.Two;
        _laserTwo = _actorsPool.GetLaser(jChain.Laser, TYPE_TWO, _count);
        SpawnPartChain(jChain, 0, jChain.Count , TYPE_TWO);
        Add(_actorsPool.GetJewel(jChain.End, 0, positionsChain.Two.Count + 1, TYPE_TWO));

        return true;
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
