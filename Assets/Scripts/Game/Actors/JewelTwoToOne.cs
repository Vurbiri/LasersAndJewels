using System;
using UnityEngine;

public class JewelTwoToOne : AJewel<JewelTwoToOne>, ILaser
{
    [Space]
    [SerializeField] private SpriteRenderer _rendererLaser;
    [SerializeField] private Sprite _spriteLOn;
    [SerializeField] private Sprite _spriteLOff;
    [Space]
    [SerializeField] private JewelCollider _jewelCollider;
    [SerializeField] private LineRenderer _laserRay;

    private Vector3[] _positionsRay;
    private readonly LoopArray<TurnData> _turnData = new(TurnData.Direction);
    private int _countVisited = 0;
    private Vector3 _position;

    public override int IdType => 0;
    public int LaserType => _idType;
    public override bool IsEnd => true;
    public override bool IsVisited { get => _countVisited >= 2; set => _countVisited = value ? _countVisited + 1 : 0; }
    public override Vector3 LocalPosition => _position;
    public Vector3[] PositionsRay => _positionsRay;

    public event Action EventSelected;

    public override void Initialize()
    {
        _jewelCollider.Initialize();
        _jewelCollider.EventClick += OnClick;
        _jewelCollider.IsInteractable = false;

        base.Initialize();
    }

    public void Setup(Vector2Int index, int idType, int maxCountRay)
    {
        BaseSetup(index, idType);

        _position = index.ToVector3();
        _thisTransform.localPosition = Vector3.zero;
        _positionsRay = new Vector3[maxCountRay];

        Color color = _colors[idType];
        _laserRay.startColor = _laserRay.endColor = color;
        _rendererLaser.color = color;
    }

    public override void Run()
    {
        _jewelCollider.LocalPosition = _position;
        _positionsRay[0] = _position;

        Turn(_turnData.Default);

        Off();
        Activate();

        _jewelCollider.IsInteractable = true;
    }

    public void SetRayPositions(int count)
    {
        _laserRay.positionCount = count;
        _laserRay.SetPositions(_positionsRay);
    }

    protected override void On(bool isLevelComplete)
    {
        if (isLevelComplete) _jewelCollider.IsInteractable = false;

        if (_isOn) return;

        _rendererLaser.sprite = _spriteLOn;
        BaseOn();
    }

    public override void Off()
    {
        if (!_isOn) return;

        _rendererLaser.sprite = _spriteLOff;
        base.Off();
    }

    private void Turn(TurnData turnData)
    {
        _jewelCollider.Rotation = turnData.Turn;
        Orientation = turnData.Orientation;
    }

    private void OnClick(bool isLeft)
    {
        Turn(isLeft ? _turnData.Back : _turnData.Forward);

        if (_isOn) EventSelected?.Invoke();
    }

    public override void Deactivate()
    {
        _laserRay.positionCount = 0;
        base.Deactivate();
    }
}
