using System;
using System.Collections;
using UnityEngine;

public class JewelOneToTwo : AJewelTo
{
    [Space]
    [SerializeField] private LaserModule _moduleOutA;
    [SerializeField] private LaserModule _moduleOutB;
    [Space]
    [SerializeField] private float _alfaRay = 0.85f;
    [SerializeField] private float _durationHideRays = 1f;
    [Space]
    [SerializeField] private JewelModule _moduleIn;

    public ILaser LaserOne => _moduleOutA;
    public ILaser LaserTwo => _moduleOutB;

    public int IdType => _idType;
    public override bool IsVisited { get => _moduleIn.IsVisited; set => _moduleIn.IsVisited = value; }

    public override void Initialize()
    {
        base.Initialize();

        _moduleOutA.Initialize(_alfaRay);
        _moduleOutB.Initialize(_alfaRay);
        _moduleIn.Initialize();
    }

    public void Setup(BranchData data, int typeOutA, int typeOutB, int typeIn, int maxCountRay)
    {
        BaseSetup(data, typeIn);

        _jewelCollider.LocalPosition = _position;

        _borderModule.Setup(_colors[typeOutA], _colors[typeOutB]);

        _moduleOutA.Setup(_index, data.OrientationOne, typeOutA, _colors[typeOutA], maxCountRay);
        _moduleOutB.Setup(_index, data.OrientationTwo, typeOutB, _colors[typeOutB], maxCountRay);
        _moduleIn.Setup(typeIn, _colors[typeIn]);

        IsVisited = false;

        Off();
    }

    public override bool ToVisit(int idType)
    {
        if (IsVisited) return false;

        return IsVisited = CheckType(idType);
    }

    public override bool CheckType(int idType) => idType == _idType;

    public override void Switch(bool isLevelComplete)
    {
        if (IsVisited)
            On(isLevelComplete);
        else
            Off();

        _moduleIn.Switch();

        IsVisited = false;
    }

    public override void ResetRays() => DeactivateModules();

    public override void Deactivate()
    {
        DeactivateModules();
        gameObject.SetActive(false);
    }

    public override IEnumerator Deactivate_Coroutine()
    {
        StartCoroutine(_moduleOutA.Deactivate_Coroutine(_durationHideRays));
        yield return StartCoroutine(_moduleOutB.Deactivate_Coroutine(_durationHideRays));
        gameObject.SetActive(false);
    }

    private void DeactivateModules()
    {
        _moduleOutA.Deactivate();
        _moduleOutB.Deactivate();
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

    protected override void Off()
    {
        if (!_isOn) return;

        _isOn = false;
        _moduleOutA.Off();
        _moduleOutB.Off();
        _borderModule.Off();
    }

    protected override void OnClick(bool isLeft)
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

        base.OnClick(isLeft);
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
        private float _alfaRay = 0.85f;

        public int LaserType => _module.IdType;
        public Vector2Int Index => _index;
        public Vector2Int Orientation => _orientation;
        public Vector3[] PositionsRay => _positionsRay;

        public void Initialize(float alfaRay)
        {
            _alfaRay = alfaRay;
            _module.Initialize();
        }

        public void Setup(Vector2Int index, Vector2Int orientation, int idType, Color color, int maxCountRay)
        {
            _index = index;
            _orientation = orientation;

            _positionsRay = new Vector3[maxCountRay];

            _laserRay.startColor = _laserRay.endColor = color.SetAlpha(_alfaRay); ;
            _module.Setup(idType, color, orientation);

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

        public IEnumerator Deactivate_Coroutine(float durationHide)
        {
            Color color = _laserRay.startColor;
            float alpha, currentTime = 0f, start = color.a;

            while (currentTime < durationHide)
            {
                alpha = Mathf.Lerp(start, 0f, currentTime / durationHide);
                _laserRay.startColor = _laserRay.endColor = color.SetAlpha(alpha);
                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }

            _laserRay.positionCount = 0;
        }
    }
    #endregion
}
