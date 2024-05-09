using UnityEngine;

public class JewelTwoToOne : AJewelInteractable<JewelTwoToOne>, ILaser
{
    [Space]
    [SerializeField] private LineRenderer _laserRay;

    protected Transform _colliderTransform;
    private Vector3[] _positionsRay;

    public override bool IsEnd => true;
    public Vector3[] PositionsRay => _positionsRay;

    public override void Initialize()
    {
        _spriteTransform = _spriteRenderer.gameObject.transform;
        _colliderTransform = _collider.transform;

        IsInteractable = false;

        base.Initialize();
    }

    public void Setup(Vector2Int index, int idType, int maxCountRay)
    {
        BaseSetup(index, idType);

        _thisTransform.localPosition = Vector3.zero;

        _positionsRay = new Vector3[maxCountRay];

        Color color = _colors[idType];
        _laserRay.startColor = _laserRay.endColor = color;
    }

    public override void Run()
    {
        _colliderTransform.localPosition = _index.ToVector3();
        _positionsRay[0] = _colliderTransform.localPosition;
        Turn(_turnData.Default);
        Off();
        Activate();
    }

    public void SetRayPositions(int count)
    {
        _laserRay.positionCount = count;
        _laserRay.SetPositions(_positionsRay);
    }
}
