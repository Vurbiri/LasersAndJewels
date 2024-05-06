using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(ActorsPool))]
public class GameArea : MonoBehaviour
{
    [SerializeField] private Vector2Int _size = new(10, 9);

    private ActorsPool _actorsPool;
    private LevelOne _levelOne;
    private LevelTwo _levelTwo;

    private int _countJewel = 55;

    private void Awake()
    {
        _actorsPool = GetComponent<ActorsPool>();
        _actorsPool.Initialize(OnSelected);

        _levelOne = new(_size, _actorsPool);
        _levelTwo = new(_size, _actorsPool);
    }

    private void Start()
    {
        _levelTwo.Create(15, 1, 15, 2, 50, 4);
        _levelTwo.Run();

        //while (!_levelOne.Create(_countJewel--, 1, 4));

        //_levelOne.Run();

    }

    private void OnSelected()
    {
        //if (_levelOne.Check())
            Debug.Log("-=/ Level complete \\=-");
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
