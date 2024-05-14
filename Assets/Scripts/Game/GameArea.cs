using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActorsPool))]
public class GameArea : MonoBehaviour
{
    [SerializeField] private Vector2Int _size = new(10, 9);
    [SerializeField] private int _countJewel = 20;
    [SerializeField] private int _baseMaxDistance = 7;
    [SerializeField] private LevelType _currentType = LevelType.TwoToOne;

    private readonly Dictionary<LevelType, ALevel> _levels = new(3);
    private ALevelCoroutine _currentLevel;

    private void Awake()
    {
        ActorsPool actorsPool = GetComponent<ActorsPool>();
        actorsPool.Initialize(OnSelected);

        _levels.Add(LevelType.One, new LevelOne(_size, actorsPool));
        _levels.Add(LevelType.Two, new LevelTwo(_size, actorsPool));
        _levels.Add(LevelType.TwoToOne, new LevelTwoToOne(_size, actorsPool));
        _levels.Add(LevelType.OneToTwo, new LevelOneToTwo(_size, actorsPool));

        //_currentLevel = _levels[_currentType];

        _currentLevel = new LevelOneCoroutine(_size, actorsPool);
    }

    //private void Start()
    //{
    //    int count = _countJewel;
    //    while (!_currentLevel.Create(count--, _baseMaxDistance - (count >> 3)));

    //    StartCoroutine(_currentLevel.Run_Coroutine());
    //}

    private IEnumerator Start()
    {
        int count = _countJewel;
        WaitResult<bool> waitResult;
        do
        yield return waitResult = _currentLevel.Generate_Wait(count--, _baseMaxDistance - (count >> 3));
        while (!waitResult.Result);
        _currentLevel.Create();
        StartCoroutine(_currentLevel.Run_Coroutine());
    }

    private void OnSelected()
    {
        if (_currentLevel.CheckChain())
            StartCoroutine(GameOver_Coroutine());
    }

    private IEnumerator GameOver_Coroutine()
    {
        Debug.Log("-=/ Level complete \\=-");
        yield return new WaitForSecondsRealtime(2f);
        yield return StartCoroutine(_currentLevel.Clear_Coroutine());
        Start();
    }


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
