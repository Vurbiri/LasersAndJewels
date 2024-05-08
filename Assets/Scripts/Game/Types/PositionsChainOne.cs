using System.Collections.Generic;

public class PositionsChainOne
{
    public LaserSimple Laser { get; }
    public List<JewelSimple> Jewels { get; }
    public JewelSimple End { get; }
    public int Count { get; }

    public PositionsChainOne(LaserSimple laser, List<JewelSimple> positions)
    {
        Laser = laser;
        End = positions.Pop();
        Jewels = positions;

        Count = positions.Count;
    }
}
