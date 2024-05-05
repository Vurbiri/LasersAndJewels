using UnityEngine;

public static class URandom
{
    public static bool IsTrue(int chanceTrue = 50) => Random.Range(0, 100) < chanceTrue;

    public static Vector2Int Vector2Int(Vector2Int minInclusive, Vector2Int maxExclusive) => new(Random.Range(minInclusive.x, maxExclusive.x), Random.Range(minInclusive.y, maxExclusive.y));
    public static Vector2Int Vector2Int(Vector2Int maxExclusive) => new(Random.Range(0, maxExclusive.x), Random.Range(0, maxExclusive.y));

    public static int[] ThreeIndexes { get => three[Random.Range(0, 6)]; }
    private static readonly int[][] three =
    {
        new int[]{ 0, 1, 2 },
        new int[]{ 0, 2, 1 },
        new int[]{ 1, 0, 2 },
        new int[]{ 1, 2, 0 },
        new int[]{ 2, 0, 1 },
        new int[]{ 2, 1, 0 },
    };
}
