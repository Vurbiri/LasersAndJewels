using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ActorsPool))]
public class GameArea : MonoBehaviour
{
    [SerializeField] private Vector2Int _size = new(10, 9);

    private ALevel _level;
    private int _countJewel = 15, _baseMaxDistance;

    private void Awake()
    {
        ActorsPool actorsPool = GetComponent<ActorsPool>();
        actorsPool.Initialize(OnSelected);

        //_level = new LevelOne(_size, actorsPool);
        _level = new LevelTwo(_size, actorsPool);

        _baseMaxDistance = Mathf.Min(_size.x, _size.y);
    }

    private void Start()
    {
        int count = _countJewel;
        while (!_level.Create(count--, _baseMaxDistance - count / 10));

        Debug.Log(count);

        _level.Run();
    }

    private void OnSelected()
    {
        if (_level.CheckChain())
            StartCoroutine(GameOver_Coroutine());
    }

    private IEnumerator GameOver_Coroutine()
    {
        Debug.Log("-=/ Level complete \\=-");
        yield return new WaitForSecondsRealtime(2f);
        _level.Clear();
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
