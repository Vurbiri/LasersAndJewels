using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActorsPool))]
public class GameArea : MonoBehaviour
{
    [SerializeField] private Vector2Int _size = new(10, 9);
    [SerializeField] private int _baseMaxDistance = 7;
    [Space]
    [SerializeField] private ScreenMessage _screenMessage;

    private readonly Dictionary<LevelType, ALevel> _levels = new(3);
    private ALevel _currentLevel, _nextLevel;
    private int _nextCount;
    private WaitResult<bool> _waitNextLevel;

    private const int MIN_COUNT = 10, MAX_COUNT = 40;

    public event Action EventLevelEnd, EventLevelStop;

    public void Initialize()
    {
        ActorsPool actorsPool = GetComponent<ActorsPool>();
        actorsPool.Initialize(OnSelected);

        _levels.Add(LevelType.One, new LevelOne(_size, actorsPool));
        _levels.Add(LevelType.Two, new LevelTwo(_size, actorsPool));
        _levels.Add(LevelType.TwoToOne, new LevelTwoToOne(_size, actorsPool));
        _levels.Add(LevelType.OneToTwo, new LevelOneToTwo(_size, actorsPool));

        _screenMessage.Initialize();
    }

    public void GenerateStartLevel(LevelType type, int count)
    {
        _currentLevel = _levels[type];

        StartCoroutine(GenerateLevel_Coroutine(Mathf.Clamp(count, MIN_COUNT, MAX_COUNT)));
    }

    public void PlayStartLevel(int level, LevelType typeNext, int countNext)
    {
        StartCoroutine(PlayLevel_Coroutine(level, typeNext, countNext));
    }

    public void PlayNextLevel(int level, LevelType typeNext, int countNext)
    {
        _currentLevel = _nextLevel;
        StartCoroutine(PlayNextLevel_Coroutine());

        #region Local: PlayNextLevel_Coroutine()
        //=================================
        IEnumerator PlayNextLevel_Coroutine()
        {
            if (_waitNextLevel.keepWaiting)
                _currentLevel.StopGenerate();

            yield return _waitNextLevel;

            if (!_waitNextLevel.Result)
                yield return StartCoroutine(GenerateLevel_Coroutine(Mathf.Clamp(_nextCount >> 1, MIN_COUNT, MAX_COUNT)));

            StartCoroutine(PlayLevel_Coroutine(level, typeNext, countNext));
        }
        #endregion
    }

    private IEnumerator PlayLevel_Coroutine(int level, LevelType typeNext, int countNext)
    {
        _screenMessage.GameLevel(level);
        _currentLevel.Create();
        yield return StartCoroutine(_currentLevel.Run_Coroutine());

        _nextLevel = _levels[typeNext];
        _nextCount = Mathf.Clamp(countNext, MIN_COUNT, MAX_COUNT);

        _waitNextLevel = _nextLevel.Generate_Wait(_nextCount, MaxDistance(_nextCount));
    }

    public IEnumerator GenerateLevel_Coroutine(int count)
    {
        while (!_currentLevel.Generate(count, MaxDistance(count)))
        {
            if (count > MIN_COUNT)
                count--;

            yield return null;
            Debug.Log(count);
        }
    }

    public bool SHowHint(float present)
    {
        if (_currentLevel == null) return false;

        return _currentLevel.ShowHint(present);
    }

    private void OnSelected()
    {
        if (_currentLevel.CheckChain())
            StartCoroutine(LevelComplete_Coroutine());
    }

    private IEnumerator LevelComplete_Coroutine()
    {
        EventLevelStop?.Invoke();

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
