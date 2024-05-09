using UnityEngine;

public static class URandom
{
    public static bool IsTrue(int chanceTrue = 50) => chanceTrue >= 100 || Random.Range(0, 100) < chanceTrue;

    public static Vector2Int Vector2Int(Vector2Int minInclusive, Vector2Int maxExclusive) => new(Random.Range(minInclusive.x, maxExclusive.x), Random.Range(minInclusive.y, maxExclusive.y));
    public static Vector2Int Vector2Int(Vector2Int maxExclusive) => new(Random.Range(0, maxExclusive.x), Random.Range(0, maxExclusive.y));
}
