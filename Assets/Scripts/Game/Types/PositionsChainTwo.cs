using System.Collections.Generic;
using UnityEngine;

public class PositionsChainTwo 
{
    public PositionsChainOne One { get; }
    public PositionsChainOne Two { get; }
    public int Connect { get; }


    public PositionsChainTwo(LaserSimple laserOne, List<Vector2Int> positionsOne, LaserSimple laserTwo, List<Vector2Int> positionsTwo)
    {
        One = new(laserOne, positionsOne);
        Two = new(laserTwo, positionsTwo);
        Connect = -1;
    }

    public PositionsChainTwo(LaserSimple laserOne, List<Vector2Int> positionsOne, LaserSimple laserTwo, List<Vector2Int> positionsTwo, int connectIndex)
    {
        One = new(laserOne, positionsOne);
        Two = new(laserTwo, positionsTwo);
        Connect = connectIndex;
    }
}
