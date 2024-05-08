using UnityEngine;

public class LevelGeneratorTwo : ALevelGeneratorTwo
{
    public LevelGeneratorTwo(Vector2Int size) : base(size) { }

    public PositionsChainTwo Generate(int countOne, int typeOne, int countTwo, int typeTwo, int chance, int maxDistance)
    {
        _area = new bool[_size.x, _size.y];
        _maxDistance = maxDistance;
        _chance = chance;

        _typeBase = _typeCurrent = typeOne;
        SetupOne(countOne);
        if (!GenerateBase()) return null;

        _typeBase = _typeCurrent = typeTwo;
        return SetupTwo(countTwo) && GenerateBase() ? new(_laserOne, _jewelsOne, _laserCurrent, _jewelsCurrent) : null;
    }
}
