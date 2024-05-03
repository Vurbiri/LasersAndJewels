using System.Collections.Generic;
using UnityEngine;

public class PositionsChain
{
    public List<Vector2Int> Positions { get; }
    public Vector2Int Start { get; }
    public Vector2Int StartOrientation { get; }
    public Vector2Int End { get; }
    public int Count { get; }
    public bool IsBuilding { get; }

    public PositionsChain(List<Vector2Int> positions, Vector2Int start, Vector2Int startOrientation, bool isBuilding)
    {
        if(!(IsBuilding = isBuilding)) return;

        Start = start;
        End = positions.Pop();
        Positions = positions;
        StartOrientation = startOrientation;

        Count = positions.Count;
    }
}
