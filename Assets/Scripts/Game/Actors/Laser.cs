using UnityEngine;

public class Laser : APooledObject<Laser>, ILaser
{
    [SerializeField] private Transform _emitter;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private LineRenderer _laserRay;
    [Space]
    [SerializeField] private Color[] _colors;

    private Vector2Int _orientation;
    private Vector2Int _index;
    private int _idType;
    private Vector3[] _positionsRay;

    public int IdType => _idType;
    public Vector2Int Index => _index;
    public Vector2Int Orientation => _orientation;
    public Vector3[] PositionsRay => _positionsRay;

    public void Setup(LaserSimple laserSimple, int idType, int maxCountRay)
    {
        _idType = idType;
        _index = laserSimple.Index;
        _orientation = laserSimple.Orientation;

        _thisTransform.localPosition = Vector3.zero;

        _laserRay.positionCount = 0;
        _positionsRay = new Vector3[maxCountRay];

        Color color = _colors[_idType];
        _laserRay.startColor = _laserRay.endColor = color;
        _spriteRenderer.color = color;
    }

    public void Run()
    {
        _emitter.localPosition = _index.ToVector3();
        _emitter.rotation = Quaternion.LookRotation(Vector3.forward, _orientation.ToVector3());
        _positionsRay[0] = _emitter.localPosition;
        Activate();
    }

    public void SetRayPositions(int count)
    {
        _laserRay.positionCount = count;
        _laserRay.SetPositions(_positionsRay);
    }
}
