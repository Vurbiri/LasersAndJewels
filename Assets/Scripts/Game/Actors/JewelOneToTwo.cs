using System;
using UnityEngine;

public class JewelOneToTwo : MonoBehaviour, IJewel, IJewelTo
{
    [SerializeField] private BorderModule _borderModule;
    [Space]
    [SerializeField] private LaserModule _moduleOutA;
    [SerializeField] private LaserModule _moduleOutB;
    [Space]
    [SerializeField] private JewelModule _moduleIn;
    [Space]
    [SerializeField] private JewelCollider _jewelCollider;


    public ILaser LaserOne => _moduleOutA;
    public ILaser LaserTwo => _moduleOutB;

    public int IdType => _idType;
    public bool IsEnd => true;
    public bool IsVisited { get => _moduleIn.IsVisited; set => _moduleIn.IsVisited = value; }
    public Vector2Int Index => _index;
    public Vector2Int Orientation => _orientation;
    public Vector3 LocalPosition => _position;

    public event Action EventSelected;

    protected GlobalColors _colors;
    private readonly LoopArray<TurnData> _turnData = new(TurnData.Direction);
    private int _idType;
    private Vector2Int _index, _orientation;
    private Vector3 _position;
    private bool _isOn = true;

    public void Initialize()
    {
        _colors = GlobalColors.InstanceF;

        _jewelCollider.Initialize();
        _jewelCollider.EventClick += OnClick;
        _jewelCollider.IsInteractable = false;

        _moduleOutA.Initialize();
        _moduleOutB.Initialize();
        _moduleIn.Initialize();

        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void Setup(BranchData data, int typeOutA, int typeOutB, int typeIn, int maxCountRay)
    {
        _idType = typeIn;
        _index = data.Index;
        
        _position = data.Position;
        _jewelCollider.LocalPosition = _position;
        Turn(_turnData.Default);

        _borderModule.Setup(_colors[typeOutA], _colors[typeOutB]);

        _moduleOutA.Setup(_index, data.OrientationOne, typeOutA, _colors[typeOutA], maxCountRay);
        _moduleOutB.Setup(_index, data.OrientationTwo, typeOutB, _colors[typeOutB], maxCountRay);
        _moduleIn.Setup(typeIn, _colors[typeIn]);

        _isOn = true;
        IsVisited = false;
    }

    public void Run()
    {
        _moduleOutA.Run();
        _moduleOutB.Run();

        Off();

        gameObject.SetActive(true);
        _jewelCollider.IsInteractable = true;
    }

    public bool ToVisit(int idType)
    {
        if (IsVisited) return false;

        return IsVisited = CheckType(idType);
    }

    public bool CheckType(int idType) => idType == _idType;

    public void Switch(bool isLevelComplete)
    {
        if (IsVisited)
            On(isLevelComplete);
        else
            Off();

        _moduleIn.Switch();

        IsVisited = false;
    }

    public void ResetRays()
    {
        _moduleOutA.SetRayPositions(0);
        _moduleOutB.SetRayPositions(0);
    }

    private void On(bool isLevelComplete)
    {
        if (isLevelComplete) _jewelCollider.IsInteractable = false;

        if (_isOn) return;

        _isOn = true;
        _moduleOutA.On();
        _moduleOutB.On();
        _borderModule.On();
    }

    private void Off()
    {
        if (!_isOn) return;

        _isOn = false;
        _moduleOutA.Off();
        _moduleOutB.Off();
        _borderModule.Off();
    }

    private void Turn(TurnData turnData)
    {
        _jewelCollider.Rotation = turnData.Turn;
        _orientation = turnData.Orientation;
    }

    private void OnClick(bool isLeft)
    {
        if(isLeft) 
        {
            Turn(_turnData.Back);
            _moduleOutA.Turn90Left();
            _moduleOutB.Turn90Left();
        }
        else
        {
            Turn(_turnData.Forward);
            _moduleOutA.Turn90Right();
            _moduleOutB.Turn90Right();
        }
        //Turn(isLeft ? _turnData.Back : _turnData.Forward);

        if (_isOn) EventSelected?.Invoke();
    }

    public void Deactivate()
    {
        _moduleOutA.Deactivate();
        _moduleOutB.Deactivate();
        gameObject.SetActive(false);
    }

    #region Nested Classe
    //***********************************
    [Serializable]
    private class LaserModule :ILaser
    {
        [SerializeField] private JewelModule _module;
        [SerializeField] private LineRenderer _laserRay;

        private Vector2Int _index, _orientation;
        private Vector3[] _positionsRay;

        public int LaserType => _module.IdType;
        public Vector2Int Index => _index;
        public Vector2Int Orientation => _orientation;
        public Vector3[] PositionsRay => _positionsRay;

        public void Initialize() => _module.Initialize();

        public void Setup(Vector2Int index, Vector2Int orientation, int idType, Color color, int maxCountRay)
        {
            _index = index;
            _orientation = orientation;

            _positionsRay = new Vector3[maxCountRay];

            _laserRay.startColor = _laserRay.endColor = color;
            _module.Setup(idType, color, orientation);

            //Debug.Log(_orientation);
        }

        public void Run()
        {
            _positionsRay[0] = _index.ToVector3();
        }

        public void SetRayPositions(int count)
        {
            _laserRay.positionCount = count;
            _laserRay.SetPositions(_positionsRay);
        }

        public void Turn90Left() => _orientation.Turn90Left();
        public void Turn90Right() => _orientation.Turn90Right();

        public void On() => _module.On();
        public void Off() => _module.Off();

        public void Deactivate()
        {
            _laserRay.positionCount = 0;
        }
    }
    #endregion
}
