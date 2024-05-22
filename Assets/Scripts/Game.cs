using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private InputController _input;
    [SerializeField] private GameArea _gameArea;
    [Space]
    [SerializeField] private float _timePreStart = 2f;
    [SerializeField] private float _timePreLevel = 0.75f;

    private WaitForSecondsRealtime _sleepPreStart, _sleepPreLevel;
    private WaitWhile _waitMenu;
    private DataGame _dataGame;
    private bool _isGame = false;

    private void Awake()
    {
        _sleepPreStart = new(_timePreStart);
        _sleepPreLevel = new(_timePreLevel);
        _waitMenu = new(() => _input.IsMenu);

        _dataGame = DataGame.Instance;

        _input.EventHint += OnHint;
        _input.EventRestart += OnRestart;

        _gameArea.EventLevelStop += OnLevelStop;
        _gameArea.EventLevelEnd += OnLevelEnd;
        
        _gameArea.Initialize(_dataGame.MinCountJewel);
        _gameArea.GenerateStartLevel(_dataGame.LevelData);
    }

    private IEnumerator Start()
    {
        yield return _sleepPreStart;
        yield return _waitMenu;
        _dataGame.StartLevel();
        _gameArea.PlayStartLevel(_dataGame.LevelData);
        _isGame = true;
    }

    private void OnLevelStop()
    {
        _isGame = false;
        _dataGame.NextLevel();
    }

    private void OnLevelEnd()
    {
        StartCoroutine(OnLevelStop_Coroutine());

        #region Local: OnLevelStop_Coroutine()
        //=================================
        IEnumerator OnLevelStop_Coroutine()
        {
            yield return _sleepPreLevel;
            yield return _waitMenu;
            _gameArea.PlayNextLevel(_dataGame.LevelData);
            _isGame = true;
        }
        #endregion
    }

    private void OnRestart()
    {
        if (_dataGame.IsNewGame)
            return;
        
        _isGame = false;
        StopAllCoroutines();
        _gameArea.Restart();
        _dataGame.ResetGame();

        _gameArea.GenerateStartLevel(_dataGame.LevelData);
        StartCoroutine(Start());
    }

    private void OnHint()
    {
        if (!_isGame || _dataGame.Hint <= 0) return;

        if (_gameArea.ShowHint())
            _dataGame.Hint--;
    }
}
