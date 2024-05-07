using Newtonsoft.Json;
using System;
using UnityEngine;

//[DefaultExecutionOrder(-1)]
public class DataGame : ASingleton<DataGame>
{
    [Space]
    [SerializeField] private string _keySave = "gmd";
    [Space]
    [Space]
    [SerializeField] private int _scoreStart = 4;
    [SerializeField] private int _scorePerLevel = 1;

    private GameSave _data;
    private bool _isNewRecord = false;


    public bool IsGameLevel => _data.modeStart == GameMode.Game;
    public bool IsRecord => _data.score > _data.maxScore;
    public int Level => _data.level;
    public int Score { get => _data.score; set { _data.score = value; EventChangeScore?.Invoke(value); } }
    public int MaxScore { get => _data.maxScore; private set { _data.maxScore = value; EventChangeMaxScore?.Invoke(value); } }

    public event Action<int> EventChangeScore;
    public event Action<int> EventChangeMaxScore;

    public bool Initialize(bool isLoad)
    {

        bool result = isLoad && Load();
        if (!result)
        {
            _data = new();
            ResetGame();
        }

        _isNewRecord = IsRecord;


        return result;
    }

    private bool Load()
    {
        Return<GameSave> data = Storage.Load<GameSave>(_keySave);
        if (data.Result)
            _data = data.Value;

        return data.Result;
    }
    public void Save(Action<bool> callback = null) => StartCoroutine(Storage.Save_Coroutine(_keySave, _data, callback));

    public void ResetGame()
    {
        if (IsRecord)
            MaxScore = Score;

        _data.Reset();
        _isNewRecord = false;
        //Save(MessageSaving);
    }

    public void ResetScoreEvent() => EventChangeScore?.Invoke(0);

    public void ScoreAdd()
    {
        Score += _scoreStart + _scorePerLevel * _data.level;

        if (!_isNewRecord && IsRecord && _data.maxScore > 0)
            _isNewRecord = true;
    }



    private void MessageSaving(bool result) => Message.Saving("GoodSave", result);

    #region Nested Classe
    //***********************************
    private class GameSave
    {
        [JsonProperty("gms")]
        public GameMode modeStart = GameMode.Game;
        [JsonProperty("lvl")]
        public int level = 1;
        [JsonProperty("scr")]
        public int score = 0;
        [JsonProperty("msc")]
        public int maxScore = 0;

        [JsonConstructor]
        public GameSave(GameMode modeStart, int level, int score, int maxScore) 
        {
            this.modeStart = modeStart;
            this.level = level;
            this.score = score;
            this.maxScore = maxScore;
        }
        public GameSave()
        {
            modeStart = GameMode.Game;
            level = 1;
            score = 0;
            maxScore = 0;
        }

        public void Reset()
        {
            modeStart = GameMode.Game;
            level = 1;
            score = 0;
        }
    }
    #endregion
}
