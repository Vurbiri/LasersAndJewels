using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Jewel : AJewel<Jewel>, IMouseClick
{
    [Space]
    [SerializeField] private TMP_Text _textCount;

    private Transform _spriteTransform;
    private BoxCollider2D _collider;

    private readonly LoopArray<TurnData> _turnData = new(4);

    public override bool IsEnd => false;
    public virtual bool IsInteractable { set => _collider.enabled = value; }

    public event Action EventSelected;

    public override void Initialize()
    {
        _spriteTransform = _spriteRenderer.gameObject.transform;
        _collider = GetComponent<BoxCollider2D>();

        IsInteractable = false;

        _turnData.Add(new(0f, Vector2Int.down));
        _turnData.Add(new(270f, Vector2Int.left));
        _turnData.Add(new(180f, Vector2Int.up));
        _turnData.Add(new(90f, Vector2Int.right));

        base.Initialize();
    }

    public void Setup(JewelSimple jewelSimple, int count, int group)
    {
        BaseSetup(jewelSimple);

        _textCount.text = count.ToString();
        _textCount.color = _colors[group].Brightness(_brightnessParticle);
    }

    public override void Run()
    {
        Turn(_turnData.Default);
        base.Run();
        IsInteractable = true;
    }

    protected override void On(bool isLevelComplete)
    {
        if (isLevelComplete) IsInteractable = false;

        BaseOn();
    }

    private void Turn(TurnData turnData) 
    {
        _spriteTransform.rotation = turnData.turn;
        Orientation = turnData.orientation;
    }

    public void OnMouseClick(bool isLeft)
    {
        Turn(isLeft ? _turnData.Back : _turnData.Forward);
        EventSelected?.Invoke();
    }

    #region Nested Classe
    //***********************************
    private class TurnData
    {
        public Quaternion turn;
        public Vector2Int orientation;

        public TurnData(float angle, Vector2Int orientation) 
        {
            turn = Quaternion.Euler(0f, 0f, angle);
            this.orientation = orientation;
        }
    }
    #endregion
}
