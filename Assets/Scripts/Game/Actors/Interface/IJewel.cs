using UnityEngine;

public interface IJewel
{
    //public int IdType { get; }
    public bool IsVisited { get; set; }
    public bool IsEnd { get; }
    public Vector2Int Index { get; }
    public Vector2Int Orientation { get; }
    public Vector3 LocalPosition { get; }

    public void Run();
    public void Switch(bool isLevelComplete);
    public bool ToVisit(int idType);
    public bool CheckType(int idType);

    public void Deactivate();
}
