using System;
using UnityEngine;

public abstract class AJewel<T> : APooledObject<T>, IJewel where T : AJewel<T>
{
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] private ParticleSystem _particle;
    [Space]
    [SerializeField] protected Color[] _colors;
    [Space]
    [SerializeField] protected float _brightnessParticle = 1.2f;
    [Space]
    [SerializeField] private float _alfaOn = 0.85f;
    [SerializeField] private Sprite _spriteOn;
    [SerializeField] private float _alfaOff = 0.55f;
    [SerializeField] private Sprite _spriteOff;

    public virtual int IdType => _idType;
    public virtual bool IsVisited { get; set; }
    public abstract bool IsEnd { get; }
    public Vector2Int Index => _index;
    public Vector2Int Orientation { get; protected set; } = Vector2Int.zero;
    public virtual Vector3 LocalPosition => _thisTransform.localPosition;

    ParticleSystem.MainModule _mainParticle;

    protected int _idType;
    protected Vector2Int _index;
    protected bool _isOn = true;
    protected Color _colorOn = Color.white;
    protected Color _colorOff = Color.gray;

    public override void Initialize()
    {
        _mainParticle = _particle.main;

        base.Initialize();
    }

    protected void BaseSetup(Vector2Int index, int idType)
    {
        _idType = idType;
        _index = index;
        
        Color color = _colors[_idType];
        _mainParticle.startColor = color.Brightness(_brightnessParticle);
        _colorOn = color.SetAlpha(_alfaOn);
        _colorOff = color.SetAlpha(_alfaOff);

        _isOn = true;
        IsVisited = false;
    }

    public virtual void Run()
    {
        _thisTransform.localPosition = _index.ToVector3();
        Off();
        Activate();
    }

    public void Switch(bool isLevelComplete)
    {
        if (IsVisited)
            On(isLevelComplete);
        else
            Off();

        IsVisited = false;
    }

    protected abstract void On(bool isLevelComplete);
    protected void BaseOn()
    {
        if (_isOn) return;
        
        _isOn = true;
        _spriteRenderer.color = _colorOn;
        _spriteRenderer.sprite = _spriteOn;
        _particle.Play();
    }

    public virtual void Off()
    {
        if (!_isOn) return;

        _isOn = false;
        _spriteRenderer.color = _colorOff;
        _spriteRenderer.sprite = _spriteOff;
        _particle.Stop();
    }

    
}
