using System;
using UnityEngine;

public abstract class AJewel<T> : APooledObject<T>, IJewel where T : AJewel<T>
{
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] private ParticleSystem _particle;
    [Space]
    [SerializeField] private Color[] _colors;
    [Space]
    [SerializeField] private float _alfaOn = 0.85f;
    [SerializeField] private Sprite _spriteOn;
    [SerializeField] private float _alfaOff = 0.55f;
    [SerializeField] private Sprite _spriteOff;

    public byte IdType => _idType;
    public Vector2Int Index => _index;
    public Vector2Int Orientation { get; protected set; } = Vector2Int.zero;
    public Vector3 LocalPosition => _thisTransform.localPosition;

    ParticleSystem.MainModule _mainParticle;

    private byte _idType;
    private Vector2Int _index;
    private bool _isOn = true, _protection = false;
    protected Color _colorOn = Color.white;
    protected Color _colorOff = Color.gray;

    public override void Initialize()
    {
        _mainParticle = _particle.main;

        base.Initialize();
    }

    protected void BaseSetup(Vector2Int index, byte idType)
    {
        _idType = idType;
        _index = index;
        
        Color color = _colors[idType];
        _mainParticle.startColor = color;
        _colorOn = color.SetAlpha(_alfaOn);
        _colorOff = color.SetAlpha(_alfaOff);

        Run();
    }

    public virtual void Run()
    {
        _thisTransform.localPosition = _index.ToVector3();
        _isOn = true;
        TurnOff();
        Activate();
    }

    public abstract void TurnOn(bool isLevelComplete);
    protected void TurnOn()
    {
        _protection = true;
        if (_isOn) return;
        
        _isOn = true;
        _spriteRenderer.color = _colorOn;
        _spriteRenderer.sprite = _spriteOn;
        _particle.Play();
    }

    public void TurnOff()
    {
        if(_protection || !_isOn)
        {
            _protection = false;
            return;
        }

        _isOn = false;
        _spriteRenderer.color = _colorOff;
        _spriteRenderer.sprite = _spriteOff;
        _particle.Stop();
    }

    
}
