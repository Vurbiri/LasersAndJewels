using System;
using UnityEngine;

public class JewelTwoToOne : MonoBehaviour, ILaser, IJewel, IJewelTo
{
    [SerializeField] protected Color[] _colors;
    [Space]
    [SerializeField] private BorderModule _borderModule;
    [Space]
    [SerializeField] private JewelModule _moduleOut;
    [SerializeField] private JewelModule _moduleInA;
    [SerializeField] private JewelModule _moduleInB;
    [Space]
    [SerializeField] private JewelCollider _jewelCollider;
    [SerializeField] private LineRenderer _laserRay;

    public int LaserType => _idType;
    public int IdType => 0;
    public bool IsEnd => true;
    public bool IsVisited { get => _countVisited >= 2; set => _countVisited = value ? _countVisited + 1 : 0; }
    public Vector2Int Index => _index;
    public Vector2Int Orientation => _orientation;
    public Vector3 LocalPosition => _position;
    public Vector3[] PositionsRay => _positionsRay;

    public event Action EventSelected;

    private Vector3[] _positionsRay;
    private readonly LoopArray<TurnData> _turnData = new(TurnData.Direction);
    private int _countVisited = 0, _idType;
    private Vector2Int _index, _orientation;
    private Vector3 _position;
    private bool _isOn = true;

    public void Initialize()
    {
        _jewelCollider.Initialize();
        _jewelCollider.EventClick += OnClick;
        _jewelCollider.IsInteractable = false;

        _moduleOut.Initialize();
        _moduleInA.Initialize();
        _moduleInB.Initialize();

        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void Setup(BranchData data, int typeOut, int typeInA, int typeInB, int maxCountRay)
    {
        _idType = typeOut;
        _index = data.Index;
        
        _position = data.Position;
        _positionsRay = new Vector3[maxCountRay];

        _jewelCollider.LocalPosition = _position;
        _positionsRay[0] = _position;
        Turn(_turnData.Default);

        Color color = _colors[typeOut];
        _laserRay.startColor = _laserRay.endColor = color;
        _borderModule.Setup(color);

        _moduleOut.Setup(typeOut, color);
        _moduleInA.Setup(typeInA, _colors[typeInA]);
        _moduleInB.Setup(typeInB, _colors[typeInB]);

        _isOn = true;
        _countVisited = 0;
    }

    public void Run()
    {
        Off();

        gameObject.SetActive(true);
        _jewelCollider.IsInteractable = true;
    }

    public void SetRayPositions(int count)
    {
        _laserRay.positionCount = count;
        _laserRay.SetPositions(_positionsRay);
    }

    public bool ToVisit(int idType)
    {
        if (IsVisited) return false;

        _countVisited++;

        if (_moduleInA.IdType == idType)
            _moduleInA.IsVisited = true;
        else if(_moduleInB.IdType == idType)
            _moduleInB.IsVisited = true;

        return idType != _idType;
    }

    public bool CheckType(int idType) => true;

    public void Switch(bool isLevelComplete)
    {
        if (IsVisited)
            On(isLevelComplete);
        else
            Off();

        _moduleInA.Switch();
        _moduleInB.Switch();

        _countVisited = 0;
    }

    private void On(bool isLevelComplete)
    {
        if (isLevelComplete) _jewelCollider.IsInteractable = false;

        if (_isOn) return;

        _isOn = true;
        _moduleOut.On();
        _borderModule.On();
    }

    private void Off()
    {
        if (!_isOn) return;

        _isOn = false;
        _moduleOut.Off();
        _borderModule.Off();
    }

    private void Turn(TurnData turnData)
    {
        _jewelCollider.Rotation = turnData.Turn;
        _orientation = turnData.Orientation;
    }

    private void OnClick(bool isLeft)
    {
        Turn(isLeft ? _turnData.Back : _turnData.Forward);

        if (_isOn) EventSelected?.Invoke();
    }

    public void Deactivate()
    {
        _laserRay.positionCount = 0;
        gameObject.SetActive(false);
    }
}
