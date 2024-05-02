using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelVisualizator : MonoBehaviour
{
    [SerializeField] private Jewel _prefabJewel;
    [SerializeField] private LaserTest _prefabLaser;

    private float _sizeArea = 920;
    private int _countSide = 8;
    private Transform _thisTransform;

    public void Initialize(float size, int countSide)
    {
        _thisTransform = transform;
        _sizeArea = size;
        _countSide = countSide;
    }

    public void Create(JewelsChain jewels)
    {
        float cellSize = _sizeArea / _countSide, startPos = cellSize / 2f;
        Vector2 cardSize = Vector2.one * cellSize;
        int count = 0;

        Instantiate(_prefabLaser, _thisTransform).Setup(LocalPosition(jewels.Laser), cardSize);

        foreach (Vector2Int index in jewels.Jewels)
            Instantiate(_prefabJewel, _thisTransform).Setup(LocalPosition(index), cardSize, index, count++);

        #region Local function
        //======================
        Vector3 LocalPosition(Vector2Int index) => new(startPos + index.x * cellSize, startPos + index.y * cellSize, 0f);
        #endregion
    }


}
