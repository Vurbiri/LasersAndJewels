using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class ALevelGeneratorCoroutine
{
    protected readonly Vector2Int _size;

    protected bool[,] _area;
    protected List<Vector2Int> _jewelsCurrent;
    protected LaserSimple _laserCurrent;
    protected int _countCurrent, _maxDistance = 8;
    protected Vector2Int _indexCurrent, _excluding;
    protected MonoBehaviour _mono;
    protected Func<bool> funcIsNotBetween;

    protected const int SHIFT_ERROR = 3;
    protected const int COUNT_ERROR = 40;

    public ALevelGeneratorCoroutine(Vector2Int size, MonoBehaviour mono)
    {
        _size = size;
        _mono = mono;
        funcIsNotBetween = IsNotBetweenOne;
    }

    protected WaitResult<bool> GenerateBase_Wait(int count, int maxDistance)
    {
        _area = new bool[_size.x, _size.y];
        _maxDistance = maxDistance;

        _countCurrent = count;
        _jewelsCurrent = new(count);

        int halfX = _size.x >> 1, halfY = _size.y >> 1;
        Vector2Int directionX, directionY;

        _indexCurrent = new(SetX(), SetY());
        _excluding = URandom.IsTrue() ? directionX : directionY;

        Add();
        _laserCurrent = new(_indexCurrent, -_excluding);
        while (IsEmpty(_laserCurrent.Move())) ;

        return GenerateChain_Wait();

        #region Local functions
        //======================
        int SetX()
        {
            if (URandom.IsTrue())
            {
                directionX = Vector2Int.left;
                return Random.Range(0, halfX);
            }
            else
            {
                directionX = Vector2Int.right;
                return Random.Range(_size.x - halfX, _size.x);
            }
        }
        int SetY()
        {
            if (URandom.IsTrue())
            {
                directionY = Vector2Int.down;
                return Random.Range(0, halfY);
            }
            else
            {
                directionY = Vector2Int.up;
                return Random.Range(_size.y - halfY, _size.y);
            }
        }
        #endregion
    }

    protected WaitResult<bool> GenerateChain_Wait()
    {
        WaitResult<bool> waitResult = new();
        _mono.StartCoroutine(GenerateChain_Coroutine());
        return waitResult;

        #region Local function
        //=================================
        IEnumerator GenerateChain_Coroutine()
        {
            bool result = false;
            int error = 0, count;
            while (_jewelsCurrent.Count < _countCurrent && error < COUNT_ERROR)
            {
                yield return null;

                if (result = TryAdd())
                {
                    Add();
                    continue;
                }

                error++;
                count = Mathf.Min((error >> SHIFT_ERROR) + 1, _jewelsCurrent.Count);
                for (int i = 0; i < count; i++)
                    RemoveLast();

                if (_jewelsCurrent.Count < 2)
                {
                    waitResult.SetResult(false);
                    yield break;
                }

                _excluding = _jewelsCurrent[^2] - _jewelsCurrent[^1];
            }

            waitResult.SetResult(result);
        }
        #endregion
    }

    protected bool TryAdd()
    {
        Vector2Int current = _jewelsCurrent[^1];
        foreach (Vector2Int direction in Direction2D.ExcludingRange(_excluding))
        {
            if (CheckAdd(current, direction))
            {
                _excluding = direction;
                return true;
            }
        }

        return false;
    }

    protected bool CheckAdd(Vector2Int start, Vector2Int direction)
    {
        Vector2Int temp = start;
        if (!IsEmpty(start += direction)) return false;

        Vector2Int end = start;
        int steps = _maxDistance;
        while (IsEmpty(end += direction) && --steps > 0) ;

        _indexCurrent = URandom.Vector2Int(start, end);
        if (funcIsNotBetween())
            return true;

        start = temp;
        temp = _indexCurrent;

        while ((_indexCurrent -= direction) != start)
            if (funcIsNotBetween())
                return true;

        _indexCurrent = temp;

        while (IsEmpty(_indexCurrent += direction))
            if (funcIsNotBetween())
                return true;

        return false;
    }

    protected bool IsNotBetweenOne() => IsNotBetween(_laserCurrent, _jewelsCurrent);

    protected bool IsNotBetween(LaserSimple laser, List<Vector2Int> jewels)
    {
        Vector2Int a = laser.Index, b = jewels[0];

        for (int i = 1; i < jewels.Count; i++)
        {
            if (_indexCurrent.IsBetween(a, b))
                return false;

            a = b; b = jewels[i];
        }

        return !_indexCurrent.IsBetween(a, b);
    }

    protected virtual void Add()
    {
        _jewelsCurrent.Add(_indexCurrent);
        _area[_indexCurrent.x, _indexCurrent.y] = true;
    }

    protected void RemoveLast()
    {
        Vector2Int index = _jewelsCurrent.Pop();
        _area[index.x, index.y] = false;
    }

    protected bool IsEmpty(Vector2Int index) => IsCorrect(index) && !_area[index.x, index.y];
    protected bool IsCorrect(Vector2Int index) => index.x >= 0 && index.x < _size.x && index.y >= 0 && index.y < _size.y;
}
