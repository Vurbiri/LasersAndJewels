using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameArea : MonoBehaviour
{
    [SerializeField] private float _size = 920f;
    [SerializeField] private int _countSide = 8;
    [Space]
    [SerializeField] private LevelVisualizator _levelVisualizator;

    private LevelGenerator _generator;

    private void Awake()
    {
        _levelVisualizator.Initialize(_size, _countSide);
        _generator = new(Vector2Int.one * _countSide);

        #region Local function
        //======================

        #endregion
    }

    private void Start()
    {
        JewelsChain chain;
        int count = 0;
        do
        {
            chain = _generator.Simple(22, 6);
            count++;
        }
        while (!chain.IsBuilding);

        Debug.Log(count);
        _levelVisualizator.Create(chain);

    }
}
