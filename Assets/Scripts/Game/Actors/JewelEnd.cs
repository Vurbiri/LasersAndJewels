using UnityEngine;

public class JewelEnd : AJewel<JewelEnd>
{
    public void Setup(Vector2Int index, byte idType) => BaseSetup(index, idType);

    public override void TurnOn(bool isLevelComplete)
    {
        if (!isLevelComplete) return;

        TurnOn();
    }
}
