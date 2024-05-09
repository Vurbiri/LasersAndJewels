using UnityEngine;

public class LaserSimple
{
    public Vector2Int Index => _index;
    public Vector2Int Orientation => _orientation;

    private Vector2Int _index;
    private readonly Vector2Int _orientation;

    public LaserSimple(Vector2Int index, Vector2Int orientation)
    {
        _index = index;
        _orientation = orientation;
    }

    public Vector2Int Move() => _index -= _orientation;

    //public static implicit operator Vector2Int(LaserSimple obj) => obj._index;
    //public static implicit operator byte(LaserSimple obj) => obj._idType;
}
