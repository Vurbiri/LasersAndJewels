using UnityEngine;

public abstract class AJewel<T> : APooledObject<T>, IJewel where T : AJewel<T>
{
    [SerializeField] protected SpriteModule _spriteModule;
    [SerializeField] private ParticleSystem _particle;
    [Space]
    [SerializeField] protected Color[] _colors;
    [Space]
    [SerializeField] protected float _brightnessParticle = 1.2f;

    public virtual int IdType => _idType;
    public virtual bool IsVisited { get => _isVisited; set => _isVisited = value; }
    public abstract bool IsEnd { get; }
    public Vector2Int Index => _index;
    public Vector2Int Orientation => _orientation;
    public virtual Vector3 LocalPosition => _thisTransform.localPosition;

    private ParticleSystem.MainModule _mainParticle;

    protected int _idType;
    protected Vector2Int _index, _orientation;
    protected bool _isOn = true, _isVisited = false, _universalType;

    public override void Initialize()
    {
        _mainParticle = _particle.main;
        _spriteModule.Initialize();

        base.Initialize();
    }

    protected void BaseSetup(Vector2Int index, int idType)
    {
        _idType = idType;
        _universalType = idType == 0;
        _index = index;
        
        Color color = _colors[_idType];
        _mainParticle.startColor = color.Brightness(_brightnessParticle);
        _spriteModule.Setup(color);

        _isOn = true;
        _isVisited = false;
    }

    public virtual void Run()
    {
        _thisTransform.localPosition = _index.ToVector3();
        Off();
        Activate();
    }

    public bool ToVisit(int idType)
    {
        if (_isVisited) return false;

        return _isVisited = CheckType(idType);
    }

    public bool CheckType(int idType) => _universalType || idType == _idType;

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
        _spriteModule.On();
        _particle.Play();
    }

    private void Off()
    {
        if (!_isOn) return;

        _isOn = false;
        _spriteModule.Off();
        _particle.Stop();
        _particle.Clear();
    }

    
}
