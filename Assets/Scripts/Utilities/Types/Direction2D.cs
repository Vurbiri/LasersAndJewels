using UnityEngine;

public static class Direction2D
{
    public static Vector2Int[] Excluding(Vector2Int direction) 
    {
        if (direction.x > 0) return directExcluding[0];
        if (direction.x < 0) return directExcluding[1];
        if (direction.y > 0) return directExcluding[2];
        if (direction.y < 0) return directExcluding[3];

        return null;
    }
    private static readonly Vector2Int[][] directExcluding =
    {
        new Vector2Int[]{ Vector2Int.down, Vector2Int.left, Vector2Int.up },
        new Vector2Int[]{ Vector2Int.up, Vector2Int.right, Vector2Int.down },
        new Vector2Int[]{ Vector2Int.right, Vector2Int.down, Vector2Int.left },
        new Vector2Int[]{ Vector2Int.left, Vector2Int.up, Vector2Int.right },
    };

    public static Vector2Int Random => line[UnityEngine.Random.Range(0, COUNT_DIRECT)];
    public static Vector2Int[] Line => line;
    private static readonly Vector2Int[] line =
    {
        Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left,
    };
    private const int COUNT_DIRECT = 4;


    public static Vector2Int[] RandomAround => allArounds[UnityEngine.Random.Range(0, COUNT_AROUND)];
    private readonly static Vector2Int[][] allArounds =
    {
        new Vector2Int[]{ Vector2Int.up, Vector2Int.right, Vector2Int.down },
        new Vector2Int[]{ Vector2Int.right, Vector2Int.down, Vector2Int.left },
        new Vector2Int[]{ Vector2Int.down, Vector2Int.left, Vector2Int.up },
        new Vector2Int[]{ Vector2Int.left, Vector2Int.up, Vector2Int.right },

        new Vector2Int[]{ Vector2Int.up, Vector2Int.left, Vector2Int.down },
        new Vector2Int[]{ Vector2Int.left, Vector2Int.down, Vector2Int.right },
        new Vector2Int[]{ Vector2Int.down, Vector2Int.right, Vector2Int.up },
        new Vector2Int[]{ Vector2Int.right, Vector2Int.up, Vector2Int.left },
    };
    private const int COUNT_AROUND = 8;
}
