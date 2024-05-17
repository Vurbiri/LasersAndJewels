using System.Collections;
using UnityEngine;

public interface IJewel
{
    public bool IsVisited { get; set; }
    public bool IsEnd { get; }
    public Vector2Int Index { get; }
    public Vector2Int Orientation { get; }
    public Vector3 LocalPosition { get; }

    public WaitActivate Run_Wait();
    public void Switch(bool isLevelComplete);
    public bool ToVisit(int idType);
    public bool CheckType(int idType);
    public bool ShowHint();

    public IEnumerator Deactivate_Coroutine();
}
