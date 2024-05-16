using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameArea _gameArea;
    [Space]
    [SerializeField] private float _timePreStart = 2f;
    [Space]
    [SerializeField] private int _countJewel = 20;
    [SerializeField] private LevelType _currentType = LevelType.TwoToOne;

    private WaitForSecondsRealtime _sleepPreStart;
    private readonly LoopArray<LevelType> _types = new(Enum<LevelType>.GetValues());

    private void Awake()
    {
        _sleepPreStart = new(_timePreStart);
        _types.SetCursor(_currentType);

        _gameArea.EventLevelStop += OnLevelStop;
        
        _gameArea.Initialize();
        _gameArea.GenerateStartLevel(_currentType, _countJewel);
    }

    private IEnumerator Start()
    {
        yield return _sleepPreStart;
        _gameArea.PlayStartLevel(_types.Forward, _countJewel);
    }

    private void OnLevelStop()
    {
        Debug.Log("-=/ Level complete \\=-");
        StartCoroutine(OnLevelStop_Coroutine());

        #region Local: OnLevelStop_Coroutine()
        //=================================
        IEnumerator OnLevelStop_Coroutine()
        {

            yield return _sleepPreStart;
            _gameArea.PlayNextLevel(_types.Forward, _countJewel);
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
}
