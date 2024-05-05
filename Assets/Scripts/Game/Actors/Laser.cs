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

    public void Setup(Vector2Int index, Vector2Int orientation, byte idType)
    {
        _idType = idType;
        _index = index;
        _orientation = orientation;
        _thisTransform.localPosition = Vector3.zero;
        _spriteTransform.localPosition = index.ToVector3();

        _laserRay.startColor = _laserRay.endColor = _colors[idType];
        //_spriteRenderer.color = color;

        Activate();
    }

    public void SetRayPositions(Vector3[] positions)
    {
        _laserRay.positionCount = positions.Length;
        _laserRay.SetPositions(positions);
    }
}
