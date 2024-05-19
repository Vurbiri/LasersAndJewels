using Newtonsoft.Json;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

//[DefaultExecutionOrder(-1)]
public class DataGame : ASingleton<DataGame>
{
    [Space]
    [SerializeField] private string _keySave = "gmd";
    [Space]
    [SerializeField] private int _minJewel = 14;
    [SerializeField] private int _maxJewel = 38;
    [SerializeField] private int _minJewelEndGame = 24;
    [Space]
    [SerializeField] private int _hintStart = 3;
    [SerializeField] private int _hintPerScore = 500;

    private GameSave _data;
    private bool _isNewRecord = false;
    private long _maxAddHint;
    private readonly LoopArray<LevelType> _types = new(Enum<LevelType>.GetValues());

    public bool IsNewGame => _data.modeStart == GameModeStart.New;
    public bool IsRecord => _data.score > _data.maxScore;
    public int Hint { get => _data.hint; set { _data.hint = value; EventChangeHint?.Invoke(value); } }
    public long Score { get => _data.score; private set { _data.score = value; EventChangeScore?.Invoke(value); } }
    public long MaxScore { get => _data.maxScore; private set { _data.maxScore = value; EventChangeMaxScore?.Invoke(value); } }
    public int MinCountJewel => _minJewel;
    public LevelData LevelData => new(_data.level, _data.type, _data.countJewel);

    public event Action<int> EventChangeHint;
    public event Action<long> EventChangeScore;
    public event Action<long> EventChangeMaxScore;

    public bool Initialize(bool isLoad)
    {
        bool result = isLoad && Load();
        if (!result)
        {
            _data = new(_minJewel, _hintStart);
            ResetGame();
        }

        _types.SetCursor(_data.type);
        _isNewRecord = _data.score > _data.maxScore;
        _maxAddHint = _data.score / _hintPerScore;

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

    public void StartLevel() => CalkTypeAndCountJewel();

    public void NextLevel()
    {
        ScoreAdd();

        _data.level++;
        CalkTypeAndCountJewel();

        Save((bool result) => Message.Saving("GoodSave", result));

        #region Local: OnLevelStop_Coroutine()
        //=================================
        void ScoreAdd()
        {
            Score += _data.countJewel;

            if (!_isNewRecord && IsRecord && _data.maxScore > 0)
                _isNewRecord = true;

            if (_data.score / _hintPerScore > _maxAddHint)
            {
                Hint++; _maxAddHint++;
            }
        }
        #endregion
    }

    public void ResetGame()
    {
        if (IsRecord)
            MaxScore = Score;

        _data.Reset(_minJewel, _hintStart);

        _isNewRecord = false;
        _types.SetCursor(_data.type);
        _maxAddHint = 0;

        Save();
    }

    private void CalkTypeAndCountJewel()
    {
        int count = _minJewel + (_data.level >> 2);

        if (count > _maxJewel)
        {
            _data.countJewel = Random.Range(_minJewelEndGame, _maxJewel + 1);
            _data.type = _types.Rand;
            return;
        }

        _data.countJewel = count;
        _data.type = _types.Forward;
    }

    #region Nested Classe
    //***********************************
    private class GameSave
    {
        [JsonProperty("gms")]
        public GameModeStart modeStart = GameModeStart.New;
        [JsonProperty("lvl")]
        public int level = 1;
        [JsonProperty("ltp")]
        public LevelType type;
        [JsonProperty("hnt")]
        public int hint = 3;
        [JsonProperty("cnt")]
        public int countJewel;
        [JsonProperty("scr")]
        public long score = 0;
        [JsonProperty("msc")]
        public long maxScore = 0;

        [JsonConstructor]
        public GameSave(GameModeStart modeStart, int level, LevelType type, int hint, int countJewel, int score, int maxScore) 
        {
            this.modeStart = modeStart;
            this.level = level;
            this.type = type;
            this.hint = hint;
            this.countJewel= countJewel;
            this.score = score;
            this.maxScore = maxScore;
        }
        public GameSave(int countJewel, int hint)
        {
            Reset(countJewel, hint);
            maxScore = 0;
        }

        public void Reset(int countJewel, int hint)
        {
            modeStart = GameModeStart.New;
            level = 1;
            type = LevelType.One;
            this.hint = hint;
            this.countJewel = countJewel;
            score = 0;
        }
    }
    #endregion
}
