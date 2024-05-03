using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Jewel : AJewel<Jewel>, IMouseClick
{
    [Space]
    [SerializeField] private TMP_Text _textCount;
    [SerializeField] private float _scaleFontSize = 2.5f;

    private Transform _spriteTransform;
    private BoxCollider2D _collider;

    private readonly LoopArray<TurnData> _turnData = new(4);

    public virtual bool IsInteractable { set => _collider.enabled = value; }

    public event Action EventSelected;

    public override void Initialize()
    {
        _spriteTransform = _spriteRenderer.gameObject.transform;
        _collider = GetComponent<BoxCollider2D>();

        IsInteractable = false;

        Vector2 size = Vector2.one * s_size;
        _collider.size = size;
        _textCount.GetComponent<RectTransform>().sizeDelta = size;
        _textCount.fontSize = s_size * _scaleFontSize;

        _turnData.Add(new(0f, Vector2Int.down));
        _turnData.Add(new(270f, Vector2Int.left));
        _turnData.Add(new(180f, Vector2Int.up));
        _turnData.Add(new(90f, Vector2Int.right));

        base.Initialize();
    }

    public void Setup(Vector2Int index, int count)
    {
        _textCount.text = count.ToString();

        Turn(_turnData.Default);
        IsInteractable = true;

        BaseSetup(index);
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
