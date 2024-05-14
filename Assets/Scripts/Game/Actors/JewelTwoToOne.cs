using System.Collections;
using UnityEngine;

public class JewelTwoToOne : AJewelTo, ILaser
{
    [Space]
    [SerializeField] private JewelModule _moduleOut;
    [SerializeField] private JewelModule _moduleInA;
    [SerializeField] private JewelModule _moduleInB;
    [Space]
    [SerializeField] private LineRenderer _laserRay;
    [SerializeField] private float _alfaRay = 0.85f;
    [SerializeField] private float _durationHide = 1f;

    public int LaserType => _idType;
    public int IdType => 0;
    public override bool IsVisited { get => _countVisited >= 2; set => _countVisited = value ? _countVisited + 1 : 0; }
    public Vector3[] PositionsRay => _positionsRay;

    private Vector3[] _positionsRay;
    private int _countVisited = 0;

    public override void Initialize()
    {
        base.Initialize();

        _moduleOut.Initialize();
        _moduleInA.Initialize();
        _moduleInB.Initialize();
    }

    public void Setup(BranchData data, int typeOut, int typeInA, int typeInB, int maxCountRay)
    {
        BaseSetup(data, typeOut);
        
        _positionsRay = new Vector3[maxCountRay];
        _positionsRay[0] = _position;

        Color color = _colors[typeOut];
        _laserRay.startColor = _laserRay.endColor = color.SetAlpha(_alfaRay);
        _borderModule.Setup(color);

        _moduleOut.Setup(typeOut, color);
        _moduleInA.Setup(typeInA, _colors[typeInA]);
        _moduleInB.Setup(typeInB, _colors[typeInB]);

        _countVisited = 0;

        Off();
    }
    public override void ResetRays() => SetRayPositions(0);

    public void SetRayPositions(int count)
    {
        _laserRay.positionCount = count;
        _laserRay.SetPositions(_positionsRay);
    }

    public override bool ToVisit(int idType)
    {
        if (IsVisited) return false;

        _countVisited++;

        if (_moduleInA.IdType == idType)
            _moduleInA.IsVisited = true;
        else if(_moduleInB.IdType == idType)
            _moduleInB.IsVisited = true;

        return idType != _idType;
    }

    public override bool CheckType(int idType) => true;

    public override void Switch(bool isLevelComplete)
    {
        if (IsVisited)
            On(isLevelComplete);
        else
            Off();

        _moduleInA.Switch();
        _moduleInB.Switch();

        _countVisited = 0;
    }

    public override void Deactivate()
    {
        _laserRay.positionCount = 0;
        gameObject.SetActive(false);
    }

    public override IEnumerator Deactivate_Coroutine()
    {
        Color color = _laserRay.startColor;
        float alpha, currentTime = 0f, start = color.a;

        while (currentTime < _durationHide)
        {
            alpha = Mathf.Lerp(start, 0f, currentTime / _durationHide);
            _laserRay.startColor = _laserRay.endColor = color.SetAlpha(alpha);
            currentTime += Time.unscaledDeltaTime;
            yield return null;
        }

        _laserRay.positionCount = 0;
        gameObject.SetActive(false);
    }

    private void On(bool isLevelComplete)
    {
        if (isLevelComplete) _jewelCollider.IsInteractable = false;

        if (_isOn) return;

        _isOn = true;
        _moduleOut.On();
        _borderModule.On();
    }

    protected override void Off()
    {
        if (!_isOn) return;

        _isOn = false;
        _moduleOut.Off();
        _borderModule.Off();
    }

    protected override void OnClick(bool isLeft)
    {
        Turn(isLeft ? _turnData.Back : _turnData.Forward);

        base.OnClick(isLeft);
    }

    
}
