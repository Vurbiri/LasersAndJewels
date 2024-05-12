using UnityEngine;

public class BorderModule : MonoBehaviour
{
    [SerializeField] protected SpriteModule _borderA;
    [SerializeField] protected SpriteModule _borderB;

    public void Setup(Color color)
    {
        _borderA.Setup(color);
        _borderB.Setup(color);
    }

    public void Setup(Color colorA, Color colorB)
    {
        _borderA.Setup(colorA);
        _borderB.Setup(colorB);
    }

    public void On()
    {
        _borderA.On();
        _borderB.On();
    }

    public void Off()
    {
        _borderA.Off();
        _borderB.Off();
    }
}
