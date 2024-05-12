using UnityEngine;

public class BranchData 
{
    public int Connect { get; }
    public Vector2Int Index { get; }
    public Vector3 Position { get; }
    public Vector2Int OrientationOne { get; }
    public Vector2Int OrientationTwo { get; }

    public BranchData (int connect, Vector2Int position, Vector2Int outV, Vector2Int newV)
    {
        Connect = connect;
        Index = position;
        Position = position.ToVector3();

        while (outV != Vector2Int.down)
        {
            outV.Turn90Left();
            newV.Turn90Left();
        }

        OrientationOne = outV;
        OrientationTwo = newV;

        //OrientationOne = Vector2Int.down;

        //if ((outV += newV) == Vector2Int.zero)
        //    OrientationTwo = Vector2Int.up;
        //else if (outV.x + outV.y == 0)
        //    OrientationTwo = Vector2Int.right;
        //else
        //    OrientationTwo = Vector2Int.left;
    }
}
