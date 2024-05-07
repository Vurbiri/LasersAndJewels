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

    public int IdType => _idType;
    public bool IsVisited { get => _isVisited; set => _isVisited = value; }
    public Vector2Int Index => _index;
    public Vector2Int Orientation { get; protected set; } = Vector2Int.zero;
    public Vector3 LocalPosition => _thisTransform.localPosition;

    ParticleSystem.MainModule _mainParticle;

    private int _idType;
    private Vector2Int _index;
    private bool _isOn = true, _isVisited = false;
    protected Color _colorOn = Color.white;
    protected Color _colorOff = Color.gray;

    public override void Initialize()
    {
        _mainParticle = _particle.main;

        base.Initialize();
    }

    protected void BaseSetup(JewelSimple jewelSimple)
    {
        _idType = jewelSimple.IdType;
        _index = jewelSimple.Index;
        
        Color color = _colors[_idType];
        _mainParticle.startColor = color.Brightness(_brightnessParticle);
        _colorOn = color.SetAlpha(_alfaOn);
        _colorOff = color.SetAlpha(_alfaOff);

        _isOn = true;
        _isVisited = false;

        Run();
    }

    public virtual void Run()
    {
        _thisTransform.localPosition = _index.ToVector3();
        Off();
        Activate();
    }

    public void Switch(bool isLevelComplete)
    {
        if (_isVisited)
            On(isLevelComplete);
        else
            Off();

        _isVisited = false;
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

    public void Off()
    {
        if (!_isOn) return;

        _isOn = false;
        _spriteRenderer.color = _colorOff;
        _spriteRenderer.sprite = _spriteOff;
        _particle.Stop();
    }

    
}
