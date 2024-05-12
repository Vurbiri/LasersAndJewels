using System.Collections.Generic;
using UnityEngine;

public class PositionsChainTwo 
{
    public PositionsChainOne One { get; }
    public PositionsChainOne Two { get; }
    public BranchData Branch { get; }


    public PositionsChainTwo(LaserSimple laserOne, List<Vector2Int> positionsOne, LaserSimple laserTwo, List<Vector2Int> positionsTwo)
    {
        One = new(laserOne, positionsOne);
        Two = new(laserTwo, positionsTwo);
        Branch = null;
    }

    public PositionsChainTwo(LaserSimple laserOne, List<Vector2Int> positionsOne, LaserSimple laserTwo, List<Vector2Int> positionsTwo, BranchData branchData)
    {
        One = new(laserOne, positionsOne);
        Two = new(laserTwo, positionsTwo);
        Branch = branchData;
    }
}
