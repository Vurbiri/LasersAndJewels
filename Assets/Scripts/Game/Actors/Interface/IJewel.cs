using UnityEngine;

public interface IJewel 
{
    public Vector2Int Index { get; }
    public Vector2Int Orientation { get; }
    public Vector3 LocalPosition { get; }

    public void On();
    public void Off();
}
