using UnityEngine;

public class LevelGeneratorOne : ALevelGenerator
{
    public LevelGeneratorOne(Vector2Int size) : base(size) { }

    public PositionsChainOne Generate(int count, int maxDistance) => GenerateBase(count, maxDistance) ? new(_laserCurrent, _jewelsCurrent) : null;


}
