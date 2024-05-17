using UnityEngine;

public interface ILaser
{
    public int LaserType { get; }
    public Vector2Int Index { get; }
    public Vector2Int Orientation { get; }
    public Vector3[] PositionsRay { get; }

    public void SetRayPositions(int count);
}
