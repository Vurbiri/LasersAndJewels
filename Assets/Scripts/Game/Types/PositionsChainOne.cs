using System.Collections.Generic;
using UnityEngine;

public class PositionsChainOne
{
    public LaserSimple Laser { get; }
    public List<Vector2Int> Jewels { get; }
    public Vector2Int End { get; }
    public int Count { get; }

    public PositionsChainOne(LaserSimple laser, List<Vector2Int> positions)
    {
        Laser = laser;
        End = positions.Pop();
        Jewels = positions;

        Count = positions.Count;
    }
}
