using UnityEngine;

public class JewelStart : AJewel<JewelStart>
{
    public void Setup(Vector2Int index, Vector2Int orientation)
    {
        Orientation = orientation;
        BaseSetup(index);
    }
}
