using System.Collections;
using UnityEngine;

public class Laser : APooledObject<Laser>, ILaser
{
    [SerializeField] private Transform _emitter;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private LineRenderer _laserRay;
    [SerializeField] private float _alfaRay = 0.85f;
    [SerializeField] private float _durationHide = 1f;

    private SoundSingleton _sound;
    private GlobalColors _colors;
    private Vector2Int _index, _orientation;
    private int _idType;
    private Vector3[] _positionsRay;

    public int LaserType => _idType;
    public Vector2Int Index => _index;
    public Vector2Int Orientation => _orientation;
    public Vector3[] PositionsRay => _positionsRay;

    public override void Initialize()
    {
        _sound = SoundSingleton.Instance;
        _colors = GlobalColors.InstanceF;

        base.Initialize();
    }

    public void Setup(LaserSimple laserSimple, int idType, int maxCountRay)
    {
        _idType = idType;
        _index = laserSimple.Index;
        _orientation = laserSimple.Orientation;

        _thisTransform.localPosition = Vector3.zero;

        _positionsRay = new Vector3[maxCountRay];

        Color color = _colors[_idType];
        _laserRay.startColor = _laserRay.endColor = color.SetAlpha(_alfaRay);
        _spriteRenderer.color = color;

        _emitter.localPosition = _index.ToVector3();
        _emitter.rotation = TurnData.TurnFromOrientation(_orientation);
        _positionsRay[0] = _emitter.localPosition;
    }

    public void Run()
    {
        Activate();
    }

    public void SetRayPositions(int count)
    {
        if (_laserRay.positionCount == 0 && count > 0)
            _sound.PlayLaser();
        
        _laserRay.positionCount = count;
        _laserRay.SetPositions(_positionsRay);
    }

    public override void Deactivate()
    {
        _laserRay.positionCount = 0;
        base.Deactivate();
    }

    public IEnumerator Deactivate_Coroutine()
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

        //_sound.PlayLaserOff();
        _laserRay.positionCount = 0;
        base.Deactivate();
    }

}
