using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Jewel : AJewelInteractable<Jewel>
{
    [Space]
    [SerializeField] private TMP_Text _textCount;

    public override void Initialize()
    {
        _spriteTransform = _spriteRenderer.gameObject.transform;

        IsInteractable = false;
        base.Initialize();
    }

    public void Setup(Vector2Int index, int idType, int count, int group)
    {
        BaseSetup(index, idType);

        _textCount.text = count.ToString();
        _textCount.color = _colors[group].Brightness(_brightnessParticle);
    }

    public override void Run()
    {
        Turn(_turnData.Default);
        base.Run();
        IsInteractable = true;
    }
}
