using UnityEngine;

public class LevelGeneratorOneToTwo : ALevelGeneratorTwo
{
    public LevelGeneratorOneToTwo(Vector2Int size) : base(size) { }

    public override PositionsChainTwo Generate(int countOne, int countTwo, int maxDistance)
    {
        if (!GenerateBase(countOne, maxDistance))
            return null;

        if (!SetupTwo(countTwo) || !GenerateChain())
            return null;

        return new(_laserOne, _jewelsOne, _laserCurrent, _jewelsCurrent, _branchData);
    }
}
