using UnityEngine;

public static class URandom
{
    public static bool IsTrue(int chanceTrue = 50) => chanceTrue >= 100 || Random.Range(0, 100) < chanceTrue;

    public static Vector2Int Vector2Int(Vector2Int minInclusive, Vector2Int maxExclusive) => new(Random.Range(minInclusive.x, maxExclusive.x), Random.Range(minInclusive.y, maxExclusive.y));
    public static Vector2Int Vector2Int(Vector2Int maxExclusive) => new(Random.Range(0, maxExclusive.x), Random.Range(0, maxExclusive.y));

    public static byte[] ThreeIndexes { get => three[Random.Range(0, 6)]; }
    private static readonly byte[][] three =
    {
        new byte[]{ 0, 1, 2 },
        new byte[]{ 0, 2, 1 },
        new byte[]{ 1, 0, 2 },
        new byte[]{ 1, 2, 0 },
        new byte[]{ 2, 0, 1 },
        new byte[]{ 2, 1, 0 },
    };

    public static byte[] FourIndexes { get => four[Random.Range(0, 18)]; }
    private static readonly byte[][] four =
    {
        new byte[]{ 0, 1, 2, 3 },
        new byte[]{ 0, 1, 3, 2 },
        new byte[]{ 0, 2, 1, 3 },
        new byte[]{ 0, 2, 3, 1 },
        new byte[]{ 0, 3, 1, 2 },
        new byte[]{ 0, 3, 2, 1 },

        new byte[]{ 1, 0, 2, 3 },
        new byte[]{ 1, 0, 3, 2 },
        new byte[]{ 1, 2, 0, 3 },
        new byte[]{ 1, 2, 3, 0 },
        new byte[]{ 1, 3, 2, 0 },
        new byte[]{ 1, 3, 0, 2 },

        new byte[]{ 2, 0, 1, 3 },
        new byte[]{ 2, 0, 3, 1 },
        new byte[]{ 2, 1, 0, 3 },
        new byte[]{ 2, 1, 3, 0 },
        new byte[]{ 2, 3, 1, 0 },
        new byte[]{ 2, 3, 0, 1 },
    };
}
