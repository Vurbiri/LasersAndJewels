using UnityEngine;

public class LevelGeneratorOne : ALevelGenerator
{
    public LevelGeneratorOne(Vector2Int size) : base(size) { }

    public PositionsChainOne Generate(int count, int type, int maxDistance)
    {
        _area = new bool[_size.x, _size.y];
        _maxDistance = maxDistance;
        _typeCurrent = type;

        SetupOne(count);
        return GenerateBase() ? new(_laserCurrent, _jewelsCurrent) : null;
    }


}
