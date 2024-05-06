using UnityEngine;

public class JewelEnd : AJewel<JewelEnd>
{
    public void Setup(JewelSimple jewelSimple) => BaseSetup(jewelSimple);

    public override void TurnOn(bool isLevelComplete)
    {
        if (!isLevelComplete) return;

        TurnOn();
    }
}
