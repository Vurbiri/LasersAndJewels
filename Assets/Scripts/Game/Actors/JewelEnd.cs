using UnityEngine;

public class JewelEnd : AJewel<JewelEnd>
{
    public void Setup(JewelSimple jewelSimple) => BaseSetup(jewelSimple);

    protected override void On(bool isLevelComplete)
    {
        if (!isLevelComplete) return;

        BaseOn();
    }
}
