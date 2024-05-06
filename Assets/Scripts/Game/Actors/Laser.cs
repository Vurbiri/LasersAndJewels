using UnityEngine;

public class Laser : APooledObject<Laser>
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private LineRenderer _laserRay;
    [Space]
    [SerializeField] private Color[] _colors;

    private Transform _spriteTransform;
    private Vector2Int _orientation;
    private Vector2Int _index;
    private byte _idType;

    public byte IdType => _idType;
    public Vector2Int Index => _index;
    public Vector2Int Orientation => _orientation;
    public Vector3 StartPosition => _spriteTransform.localPosition;

    public override void Initialize()
    {
        base.Initialize();
        _spriteTransform = _spriteRenderer.transform;
    }

    public void Setup(LaserSimple laserSimple)
    {
        _idType = laserSimple.IdType;
        _index = laserSimple.Index;
        _orientation = laserSimple.Orientation;

        _thisTransform.localPosition = Vector3.zero;
        _spriteTransform.localPosition = _index.ToVector3();

        _laserRay.startColor = _laserRay.endColor = _colors[_idType];
        //_spriteRenderer.color = color;

        Activate();
    }

    public void SetRayPositions(Vector3[] positions)
    {
        _laserRay.positionCount = positions.Length;
        _laserRay.SetPositions(positions);
    }
}
