using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameArea _gameArea;
    [Space]
    [SerializeField] private float _percentCountHint = 0.3f;
    [Space]
    [SerializeField] private float _timePreStart = 2f;
    [Space]
    [SerializeField] private int _countJewel = 20;
    [SerializeField] private LevelType _currentType = LevelType.TwoToOne;

    private WaitForSecondsRealtime _sleepPreStart;
    private readonly LoopArray<LevelType> _types = new(Enum<LevelType>.GetValues());

    private int _currentLevel = 1;

    private void Awake()
    {
        _sleepPreStart = new(_timePreStart);
        _types.SetCursor(_currentType);

        _gameArea.EventLevelEnd += OnLevelEnd;
        
        _gameArea.Initialize();
        _gameArea.GenerateStartLevel(_currentType, _countJewel);
    }

    private IEnumerator Start()
    {
        yield return _sleepPreStart;
        _gameArea.PlayStartLevel(_currentLevel++, _types.Forward, _countJewel);
    }

    private void OnLevelEnd()
    {
        StartCoroutine(OnLevelStop_Coroutine());

        #region Local: OnLevelStop_Coroutine()
        //=================================
        IEnumerator OnLevelStop_Coroutine()
        {

            yield return _sleepPreStart;
            _gameArea.PlayNextLevel(_currentLevel++, _types.Forward, _countJewel);
        }
        #endregion

    }

    private void OnPause()
    {

        Time.timeScale = 0;
    }

    private void OnPlay()
    {
        Time.timeScale = 1;

    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            _gameArea.SHowHint(_percentCountHint);
        }
    }
#endif
}
