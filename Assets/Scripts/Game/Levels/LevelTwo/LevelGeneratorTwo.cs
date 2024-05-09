using UnityEngine;

public class LevelGeneratorTwo : ALevelGeneratorTwo
{
    public LevelGeneratorTwo(Vector2Int size) : base(size) { }

    public PositionsChainTwo Generate(int countOne, int countTwo, int maxDistance)
    {
        if (!GenerateBase(countOne, maxDistance))
            return null;

        return SetupTwo(countTwo) && GenerateChain() ? new(_laserOne, _jewelsOne, _laserCurrent, _jewelsCurrent) : null;
    }

    protected new bool SetupTwo(int count)
    {
        funcIsNotBetween = IsNotBetweenTwo;

        _laserOne = _laserCurrent;
        _jewelsOne = _jewelsCurrent;

        _countCurrent = count;
        _jewelsCurrent = new(count);

        int error = 0;
        count = COUNT_ERROR >> 1;

        while (error++ < count)
        {
            if (IsEmpty(_indexCurrent = URandom.Vector2Int(_size)) && IsNotBetween(_laserOne, _jewelsOne) && SetupLaserTwo())
            {
                Add();
                return true;
            }
        }
        return false;
    }

    
}
