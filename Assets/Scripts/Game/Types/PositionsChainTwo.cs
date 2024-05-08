using System.Collections.Generic;

public class PositionsChainTwo 
{
    public PositionsChainOne One { get; }
    public PositionsChainOne Two { get; }

    public PositionsChainTwo(LaserSimple laserOne, List<JewelSimple> positionsOne, LaserSimple laserTwo, List<JewelSimple> positionsTwo)
    {
        One = new(laserOne, positionsOne);
        Two = new(laserTwo, positionsTwo);
    }
}
