using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public interface ILaser
{
    public int IdType { get; }
    public Vector2Int Index { get; }
    public Vector2Int Orientation { get; }
    public Vector3[] PositionsRay { get; }

    public void SetRayPositions(int count);
}
