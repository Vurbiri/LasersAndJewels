using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class AJewel<T> : APooledObject<T>, IJewel where T : AJewel<T>
{
    [SerializeField] protected SpriteModule _spriteModule;
    [SerializeField] private ParticleSystem _particle;
    [Space]
    [SerializeField] protected float _brightnessParticle = 1.2f;
    [Space]
    [SerializeField] private Vector2 _timeShowHide = new(0.5f, 1.5f);

    public virtual int IdType => _idType;
    public virtual bool IsVisited { get => _isVisited; set => _isVisited = value; }
    public abstract bool IsEnd { get; }
    public Vector2Int Index => _index;
    public Vector2Int Orientation => _orientation;
    public virtual Vector3 LocalPosition => _thisTransform.localPosition;

    private ParticleSystem.MainModule _mainParticle;

    protected GlobalColors _colors;
    protected int _idType;
    protected Vector2Int _index, _orientation;
    protected bool _isOn = true, _isVisited = false, _universalType;

    public override void Initialize()
    {
        _mainParticle = _particle.main;
        _colors = GlobalColors.InstanceF;

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

        _thisTransform.localPosition = _index.ToVector3();

        _isOn = false;
        _isVisited = false;
    }

    public virtual void Run()
    {
        Activate();
    }

    public WaitActivate Run_Wait()
    {
        WaitActivate wait = new();
        Activate();
        StartCoroutine(Run_Coroutine());
        return wait;

        #region Local function
        //=================================
        IEnumerator Run_Coroutine()
        {
            yield return new WaitForSecondsRealtime(URandom.Range(_timeShowHide));
            yield return StartCoroutine(_spriteModule.Appear());
            Run_Wait_FinalAction();
            wait.Activate();
        }
        #endregion
    }

    protected virtual void Run_Wait_FinalAction() { }

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

    public override void Deactivate()
    {
        Off();
        base.Deactivate();
    }

    public virtual IEnumerator Deactivate_Coroutine()
    {
        yield return new WaitForSecondsRealtime(URandom.Range(_timeShowHide));
        _particle.Stop();
        _particle.Clear();
        yield return StartCoroutine(_spriteModule.Fade());

        base.Deactivate();
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
