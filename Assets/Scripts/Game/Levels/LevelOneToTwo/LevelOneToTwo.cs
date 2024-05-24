using UnityEngine;

public class LevelOneToTwo : ALevelTwo
{
    private JewelOneToTwo _jewelOneToTwo;
    protected override int StartFromRandom => 1;
    protected override int EndFromRandom => _count >> 2;


    public override LevelType Type => LevelType.OneToTwo;
    
    public LevelOneToTwo(Vector2Int size, ActorsPool actorsPool) : base(size, actorsPool)
    {
        _generator = new LevelGeneratorOneToTwo(size);
        _generatorC = new LevelGeneratorOneToTwoC(size, actorsPool);
    }

    public override void Create()
    {
        _colorGenerator.GenerateThree();
        _count = _positionsChain.One.Count + _positionsChain.Two.Count + 1;
        _jewels = new(_count);

        int connect = _positionsChain.Branch.Connect;
        PositionsChainOne jChain = _positionsChain.One;
        _laserOne = _actorsPool.GetLaser(jChain.Laser, TYPE_THREE, _count);
        SpawnPartChain(jChain, 0, connect, TYPE_THREE, false, true);

        Add(_jewelOneToTwo = _actorsPool.GetJewelOneToTwo(_positionsChain.Branch, TYPE_ONE, TYPE_TWO, TYPE_THREE, _count));
        
        SpawnPartChain(jChain, connect + 1, jChain.Count, TYPE_ONE, true);
        Add(_actorsPool.GetJewelEnd(jChain.End, TYPE_ONE));

        jChain = _positionsChain.Two;
        SpawnPartChain(jChain, 0, jChain.Count, TYPE_TWO, true);
        Add(_actorsPool.GetJewelEnd(jChain.End, TYPE_TWO));
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
