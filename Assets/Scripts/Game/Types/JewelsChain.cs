using System.Collections.Generic;
using UnityEngine;

public class JewelsChain
{
    public List<Vector2Int> Jewels { get; }
    public Vector2Int Laser { get; }
    public int Count { get; }
    public bool IsBuilding { get; }

    public JewelsChain(List<Vector2Int> jewels, Vector2Int laser, bool isBuilding)
    {
        Jewels = jewels;
        Laser = laser;
        Count = jewels.Count;

        IsBuilding = isBuilding;
    }
}
