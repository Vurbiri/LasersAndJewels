using UnityEngine;

public class Laser : APooledObject<Laser>
{
    [SerializeField] private Transform _emitter;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private LineRenderer _laserRay;
    [Space]
    [SerializeField] private Color[] _colors;

    //private Transform _spriteTransform;
    private Vector2Int _orientation;
    private Vector2Int _index;
    private int _idType;
    private Vector3[] _positionsRay;

    public int IdType => _idType;
    public Vector2Int Index => _index;
    public Vector2Int Orientation => _orientation;
    public Vector3[] PositionsRay => _positionsRay;

    public override void Initialize()
    {
        base.Initialize();
        //_spriteTransform = _spriteRenderer.transform;
    }

    public void Setup(LaserSimple laserSimple, int maxCountRay)
    {
        _idType = laserSimple.IdType;
        _index = laserSimple.Index;
        _orientation = laserSimple.Orientation;

        _thisTransform.localPosition = Vector3.zero;
        _emitter.localPosition = _index.ToVector3();
        _emitter.rotation = Quaternion.LookRotation(Vector3.forward, _orientation.ToVector3());

        _positionsRay = new Vector3[maxCountRay];
        _positionsRay[0] = _emitter.localPosition;

        Color color = _colors[_idType];
        _laserRay.startColor = _laserRay.endColor = color;
        _spriteRenderer.color = color;

        Activate();
    }

    public void SetRayPositions(int count)
    {
        _laserRay.positionCount = count;
        _laserRay.SetPositions(_positionsRay);
    }
}
