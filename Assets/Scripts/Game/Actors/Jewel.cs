using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Jewel : MonoBehaviour, IMouseClick
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [Space]
    [SerializeField] private TMP_Text _textCount;
    [SerializeField] private float _scaleFontSize = 2.5f;

    private Transform _thisTransform;
    private Transform _spriteTransform;
    private BoxCollider2D _collider;
    private RectTransform _textRectTransform;

    private readonly Loop<TurnData> _turnData = new(4);

    public Vector2Int Index { get; private set; }
    public Vector2Int Orientation { get; private set; }

    public virtual bool IsInteractable { set => _collider.enabled = value; }

    public event Action<Jewel> EventSelected;

    private void Awake()
    {
        _thisTransform = transform;
        _spriteTransform = _spriteRenderer.gameObject.transform;
        _textRectTransform = _textCount.GetComponent<RectTransform>();
        _collider = GetComponent<BoxCollider2D>();
        IsInteractable = false;

        _turnData.Add(new(0f, Vector2Int.down));
        _turnData.Add(new(270f, Vector2Int.left));
        _turnData.Add(new(180f, Vector2Int.up));
        _turnData.Add(new(90f, Vector2Int.right));
    }

    public void Setup(Vector3 position, Vector2 size, Vector2Int index, int count)
    {
        Index = index;

        _thisTransform.localPosition = position;
        _spriteRenderer.size = size;

        _collider.size = size;

        _textRectTransform.sizeDelta = size;
        _textCount.fontSize = size.x * _scaleFontSize;

        _textCount.text = count.ToString();

        Turn(_turnData.Default);
        IsInteractable = true;
    }

    private void Turn(TurnData turnData) 
    {
        _spriteTransform.localRotation = turnData.turn;
        Orientation = turnData.orientation;
    }

    public void OnMouseClick(bool isLeft)
    {
        Turn(isLeft ? _turnData.Forward : _turnData.Back);
        EventSelected?.Invoke(this);
    }

    //private void OnMouseDown()
    //{
    //    Turn(_turnData.Forward);
    //    EventSelected?.Invoke(this);
    //}


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
