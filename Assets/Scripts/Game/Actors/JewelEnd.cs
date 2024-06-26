using UnityEngine;

public class JewelEnd : AJewel<JewelEnd>
{
    public override bool IsEnd => true;

    public void Setup(Vector2Int index, int idType) => BaseSetup(index, idType);

    public override bool ShowHint() => false;

    protected override void On(bool isLevelComplete)
    {
        if (!isLevelComplete) return;

        BaseOn();
    }
}
