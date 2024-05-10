using UnityEngine;

public class LevelGeneratorTwoToOne : ALevelGeneratorTwo
{
    public LevelGeneratorTwoToOne(Vector2Int size) : base(size) { }

    public PositionsChainTwo Generate(int countOne, int countTwo, int maxDistance)
    {
        if (!GenerateBase(countOne, maxDistance))
            return null;

        if (!SetupTwo(countTwo - 1) || !GenerateChain() || !AddLaserTwo())
            return null;

        _jewelsCurrent.Reverse();

        return  new(_laserOne, _jewelsOne, _laserCurrent, _jewelsCurrent, _connectIndex);

        #region Local functions
        //======================
        bool AddLaserTwo()
        {
            while(_jewelsCurrent.Count > 2)
            {
                _excluding = _jewelsCurrent[^2] - _jewelsCurrent[^1];

                if (TryAdd() && SetupLaserTwo())
                {
                    Add();
                    return true;
                }

                RemoveLast();
            }

            return false;
        }
        #endregion
    }

    
}
