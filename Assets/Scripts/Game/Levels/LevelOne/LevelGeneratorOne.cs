using UnityEngine;

public class LevelGeneratorOne : ALevelGenerator
{
    public LevelGeneratorOne(Vector2Int size) : base(size) { }

    public PositionsChainSimple Generate(int count, int type, int maxDistance)
    {
        _area = new bool[_size.x, _size.y];
        _maxDistance = maxDistance;
        _type = type;

        SetupOne(count);
        return GenerateOne() ? new(_laser, _jewels) : null;
    }


}
