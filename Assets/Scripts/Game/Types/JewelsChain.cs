using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelsChain : IEnumerable<IJewel>
{
    public HashSet<IJewel> Jewels { get; }
    public Vector3 Last { get; }
    public bool IsLast { get; }
    public int Count { get; }

    public JewelsChain(HashSet<IJewel> jewels, Vector2Int last, bool isLast)
    {
        Jewels = jewels;
        Last = last.ToVector3();
        IsLast = isLast;

        Count = jewels.Count;
    }

    public IEnumerator<IJewel> GetEnumerator() => Jewels.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Jewels.GetEnumerator();
}
