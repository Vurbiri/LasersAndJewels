using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ActorsPool))]
public class GameArea : MonoBehaviour
{
    [SerializeField] private Vector2Int _size = new(10, 9);

    private readonly Dictionary<LevelType, ALevel> _levels = new(3);
    private ALevel _currentLevel;
    private int _countJewel = 20, _baseMaxDistance;

    private void Awake()
    {
        ActorsPool actorsPool = GetComponent<ActorsPool>();
        actorsPool.Initialize(OnSelected);

        _levels.Add(LevelType.LevelOne, new LevelOne(_size, actorsPool));
        _levels.Add(LevelType.LevelTwo, new LevelTwo(_size, actorsPool));
        _levels.Add(LevelType.LevelTwoToOne, new LevelTwoToOne(_size, actorsPool));

        _currentLevel = _levels[LevelType.LevelTwo];

        _baseMaxDistance = Mathf.Min(_size.x, _size.y);
    }

    private void Start()
    {
        int count = _countJewel;
        while (!_currentLevel.Create(count--, _baseMaxDistance - count / 10));

        Debug.Log(count);

        _currentLevel.Run();
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
        _currentLevel.Clear();
        Start();
    }
    

#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.red;
        Vector3 size = _size.ToVector3();
        Gizmos.DrawWireCube(transform.position + size * 0.5f, size);
    }

#endif
}
