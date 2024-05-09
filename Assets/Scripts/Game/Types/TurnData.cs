using UnityEngine;

public class TurnData 
{
    public Quaternion Turn { get; }
    public Vector2Int Orientation { get; }

    public static TurnData[] Direction => _direction;
    private static readonly TurnData[] _direction = 
    { 
        new(0f, Vector2Int.down),
        new(270f, Vector2Int.left),
        new(180f, Vector2Int.up), 
        new(90f, Vector2Int.right)
    };
    
    public TurnData(float angle, Vector2Int orientation)
    {
        Turn = Quaternion.Euler(0f, 0f, angle);
        this.Orientation = orientation;
    }
}
