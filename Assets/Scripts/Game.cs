using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private InputController _input;
    [SerializeField] private GameArea _gameArea;
    [Space]
    [SerializeField] private float _timePreStart = 2f;

    private WaitForSecondsRealtime _sleepPreStart;
    private DataGame _dataGame;

    private void Awake()
    {
        _sleepPreStart = new(_timePreStart);

        _dataGame = DataGame.Instance;

        _input.EventHint += OnHint;

        _gameArea.EventLevelEnd += OnLevelEnd;
        
        _gameArea.Initialize(_dataGame.MinCountJewel);
        _gameArea.GenerateStartLevel(_dataGame.LevelData);
    }

    private IEnumerator Start()
    {
        yield return _sleepPreStart;
        _dataGame.StartLevel();
        _gameArea.PlayStartLevel(_dataGame.LevelData);
        _input.IsHint = _dataGame.Hint > 0;
    }

    private void OnLevelEnd()
    {
        _dataGame.NextLevel();
        StartCoroutine(OnLevelStop_Coroutine());

        #region Local: OnLevelStop_Coroutine()
        //=================================
        IEnumerator OnLevelStop_Coroutine()
        {
            yield return _sleepPreStart;
            _gameArea.PlayNextLevel(_dataGame.LevelData);
            _input.IsHint = _dataGame.Hint > 0;
        }
        #endregion
    }

    private void OnHint()
    {
        if (_dataGame.Hint <= 0) return;

        if (_gameArea.ShowHint())
        {
            _dataGame.Hint--;
            _input.IsHint = false;
        }
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
