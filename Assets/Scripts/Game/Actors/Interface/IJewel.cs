using UnityEngine;

public interface IJewel
{
    public byte IdType { get; }
    public Vector2Int Index { get; }
    public Vector2Int Orientation { get; }
    public Vector3 LocalPosition { get; }

    public void Run();

    public void TurnOn(bool isLevelComplete);
    public void TurnOff();

    public void Deactivate();
}
