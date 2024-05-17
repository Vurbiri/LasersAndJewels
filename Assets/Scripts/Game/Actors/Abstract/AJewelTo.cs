using System;
using System.Collections;
using UnityEngine;

public abstract class AJewelTo : MonoBehaviour, IJewel
{
    [SerializeField] protected BorderModule _borderModule;
    [Space]
    [SerializeField] protected JewelCollider _jewelCollider;

    public abstract bool IsVisited { get; set; }
    public bool IsEnd => true;
    public Vector2Int Index => _index;
    public Vector2Int Orientation => _orientation;
    public Vector3 LocalPosition => _position;

    public event Action EventSelected;

    protected SoundSingleton _sound;
    protected GlobalColors _colors;
    protected readonly LoopArray<TurnData> _turnData = new(TurnData.Direction);
    protected int _idType;
    protected Vector2Int _index, _orientation;
    protected Vector3 _position;
    protected bool _isOn = true;
    private readonly WaitForSecondsRealtime _sleep = new(2f);
    
    public virtual void Initialize()
    {
        _sound = SoundSingleton.Instance;
        _colors = GlobalColors.InstanceF;

        _jewelCollider.Initialize();
        _jewelCollider.EventClick += OnClick;
        _jewelCollider.IsInteractable = false;

        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }

    public bool ShowHint() => false;

    public WaitActivate Run_Wait() 
    {
        WaitActivate wait = new();
        gameObject.SetActive(true);
        StartCoroutine(Run_Coroutine());
        return wait;

        #region Local function
        //=================================
        IEnumerator Run_Coroutine()
        {
            yield return _sleep;
            _jewelCollider.IsInteractable = true;

            wait.Activate();
        }
        #endregion
    }

    public abstract void Switch(bool isLevelComplete);
    public abstract bool ToVisit(int idType);
    public abstract bool CheckType(int idType);
    public abstract void ResetRays();
    public abstract void Deactivate();
    public abstract IEnumerator Deactivate_Coroutine();

    protected void BaseSetup(BranchData data, int type)
    {
        _idType = type;
        _index = data.Index;
        _position = data.Position;

        _jewelCollider.LocalPosition = _position;
        Turn(_turnData.Default);

        _isOn = true;
    }

    protected abstract void Off();

    protected void Turn(TurnData turnData)
    {
        _jewelCollider.Rotation = turnData.Turn;
        _orientation = turnData.Orientation;
    }

    protected virtual void OnClick(bool isLeft)
    {
        _sound.PlayTurn();

        if (_isOn) EventSelected?.Invoke();
    }
}
