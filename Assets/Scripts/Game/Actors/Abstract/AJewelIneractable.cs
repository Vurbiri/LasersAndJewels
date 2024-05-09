using System;
using UnityEngine;

public class AJewelInteractable<T> : AJewel<T>, IMouseClick where T : AJewel<T>
{
    [Space]
    [SerializeField] protected BoxCollider2D _collider;
    protected Transform _spriteTransform;

    protected readonly LoopArray<TurnData> _turnData = new(TurnData.Direction);

    public override bool IsEnd => false;
    public virtual bool IsInteractable { set => _collider.enabled = value; }

    public event Action EventSelected;

    protected override void On(bool isLevelComplete)
    {
        if (isLevelComplete) IsInteractable = false;

        BaseOn();
    }

    protected void Turn(TurnData turnData)
    {
        _spriteTransform.rotation = turnData.Turn;
        Orientation = turnData.Orientation;
    }

    public void OnMouseClick(bool isLeft)
    {
        Turn(isLeft ? _turnData.Back : _turnData.Forward);
        EventSelected?.Invoke();
    }
}
