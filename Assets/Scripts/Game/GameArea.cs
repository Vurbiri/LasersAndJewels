using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActorsPool))]
public class GameArea : MonoBehaviour
{
    [SerializeField] private Vector2Int _size = new(10, 9);
    [Space]
    [SerializeField] private float _percentCountHint = 0.375f;
    [SerializeField] private int _baseMaxDistance = 7;
    [Space]
    [SerializeField] private ScreenMessage _screenMessage;

    private readonly Dictionary<LevelType, ALevel> _levels = new(3);
    private ALevel _currentLevel, _nextLevel;
    private int _nextCount, _minCount;
    private WaitResult<bool> _waitNextLevel;

    public event Action EventLevelEnd;

    public void Initialize(int minCount)
    {
        _minCount = minCount;

        ActorsPool actorsPool = GetComponent<ActorsPool>();
        actorsPool.Initialize(OnSelected);

        _levels.Add(LevelType.One, new LevelOne(_size, actorsPool));
        _levels.Add(LevelType.Two, new LevelTwo(_size, actorsPool));
        _levels.Add(LevelType.TwoToOne, new LevelTwoToOne(_size, actorsPool));
        _levels.Add(LevelType.OneToTwo, new LevelOneToTwo(_size, actorsPool));

        _screenMessage.Initialize();
    }

    public void GenerateStartLevel(LevelData data)
    {
        _currentLevel = _levels[data.TypeNext];
        StartCoroutine(GenerateCurrent_Coroutine(data.CountNext));
    }

    public void PlayStartLevel(LevelData data)
    {
        StartCoroutine(PlayLevel_Coroutine(data));
    }

    public void PlayNextLevel(LevelData data)
    {
        _currentLevel = _nextLevel;
        StartCoroutine(PlayNextLevel_Coroutine());

        #region Local: PlayNextLevel_Coroutine()
        //=================================
        IEnumerator PlayNextLevel_Coroutine()
        {
            _waitNextLevel.EventCompleted -= OnCompleted;

            if (_waitNextLevel.keepWaiting)
                _currentLevel.StopGenerate();

            yield return _waitNextLevel;

            if (!_waitNextLevel.Result)
                yield return StartCoroutine(GenerateCurrent_Coroutine(Mathf.Clamp(_nextCount - 1, _minCount, _nextCount)));

            StartCoroutine(PlayLevel_Coroutine(data));
        }
        #endregion
    }

    private IEnumerator PlayLevel_Coroutine(LevelData data)
    {
        _screenMessage.GameLevel(data.Level);
        _currentLevel.Create();
        yield return StartCoroutine(_currentLevel.Run_Coroutine());

        _nextLevel = _levels[data.TypeNext];
        _nextCount = data.CountNext;
        GenerateNext();
    }

    public IEnumerator GenerateCurrent_Coroutine(int count)
    {
        while (!_currentLevel.Generate(count, MaxDistance(count)))
        {
            if (count > _minCount)
                count--;

            yield return null;
        }
    }

    public bool ShowHint()
    {
        if(_currentLevel != null && _currentLevel.ShowHint(_percentCountHint))
        {
            _screenMessage.Hint();
            return true;
        }

        return false;
    }

    private void GenerateNext()
    {
        _waitNextLevel = _nextLevel.Generate_Wait(_nextCount, MaxDistance(_nextCount));
        _waitNextLevel.EventCompleted += OnCompleted;
    }

    private void OnCompleted(bool result)
    {
        //_waitNextLevel.EventCompleted -= OnCompleted;

        if (result) return;

        if (_nextCount > _minCount)
            _nextCount--;

        GenerateNext();
    }

    private void OnSelected()
    {
        if (_currentLevel.CheckChain())
            StartCoroutine(LevelComplete_Coroutine());
    }

    private IEnumerator LevelComplete_Coroutine()
    {
        _screenMessage.LevelComplete();
        yield return StartCoroutine(_currentLevel.Clear_Coroutine());
       
        EventLevelEnd?.Invoke();
    }

    private int MaxDistance(int count) => _baseMaxDistance - (count >> 3);


#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.red;
        Vector3 size = _size.ToVector3();
        Gizmos.DrawWireCube(transform.position + size * 0.5f, size);
    }

#endif
}
