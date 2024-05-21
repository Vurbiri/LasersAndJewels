using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ALevel
{
    protected readonly GlobalColors _colorGenerator;
    protected readonly ActorsPool _actorsPool;
    protected readonly Vector2Int _size;

    protected IJewel[,] _area;
    protected List<IJewel> _jewels;
    protected Laser _laserOne;
    protected int _count;
    protected bool _isGeneration, _allowedHint;

    protected IJewel this[Vector2Int index] { get => _area[index.x, index.y]; set => _area[index.x, index.y] = value; }
    protected virtual int RemoveCountHint => 1;

    public abstract LevelType Type { get; }
    public bool IsGeneration => _isGeneration;

    protected const int SHIFT_ATTEMPS = 2;
    protected const int TYPE_ZERO = 0, TYPE_ONE = 1, TYPE_TWO = 2, TYPE_THREE = 3;
    protected const int CHANCE_BASE = 35;

    public ALevel(Vector2Int size, ActorsPool actorsPool)
    {
        _size = size;
        _area = new IJewel[size.x, size.y];
        _actorsPool = actorsPool;
        _colorGenerator = GlobalColors.InstanceF;
    }

    public abstract bool Generate(int count, int maxDistance);
    public abstract WaitResult<bool> Generate_Wait(int count, int maxDistance);
    public abstract void Create();

    public void StopGenerate() => _isGeneration = false;

    public virtual IEnumerator Run_Coroutine()
    {
        WaitAll waitAll = new(_actorsPool);
        _laserOne.Run();
        _jewels.ForEach((j) => waitAll.Add(j.Run_Wait()));
        yield return waitAll;
        CheckChain();
        _allowedHint = true;
    }

    public abstract bool CheckChain();

    public virtual void Reset()
    {
        _actorsPool.StopAllCoroutines();
        _allowedHint = false;
        
        if(_laserOne == null)
            return;

        _area = new IJewel[_size.x, _size.y];
        _laserOne.Deactivate();
        _jewels.ForEach((j) => j.Deactivate());
        _laserOne = null;
        _jewels = null;
    }

    public virtual IEnumerator Clear_Coroutine()
    {
        _allowedHint = false;
        _area = new IJewel[_size.x, _size.y];

        WaitAll waitAll = new(_actorsPool);
        waitAll.Add(_laserOne.Deactivate_Coroutine());
        _jewels.ForEach((j) => waitAll.Add(j.Deactivate_Coroutine()));
        yield return waitAll;
        _laserOne = null;
        _jewels = null;
    }

    public bool ShowHint(float present)
    {
        if (!_allowedHint) return false;

        _allowedHint = false;
        int count = Mathf.RoundToInt((_count - RemoveCountHint) * present);

        while (count > 0) 
            if (_jewels.Rand().ShowHint())
                count--;

        return true;
    }

    protected int CheckChain(ILaser laser)
    {
        int count = 1, currentType = laser.LaserType;

        Vector2Int direction = laser.Orientation, index = laser.Index, directionOld = -direction;
        IJewel current = null;

        do
        {
            while (IsEmpty(index += direction)) ;

            if (!IsCorrect(index)) break;

            current = this[index];
            directionOld = direction;
            direction = current.Orientation;
        }
        while (ToVisit() && !current.IsEnd);

        int countVisited = count - 1;

        if (current == null || (!current.CheckType(currentType) || (!current.IsEnd && directionOld != -direction)))
            laser.PositionsRay[count++] = index.ToVector3();

        laser.SetRayPositions(count);

        return countVisited;

        #region Local: ToVisit()
        //======================
        bool ToVisit()
        {
            if (!current.ToVisit(currentType)) return false;

            laser.PositionsRay[count++] = current.LocalPosition;
            return true;
        }
        #endregion
    }

    protected void Add(IJewel jewel)
    {
        this[jewel.Index] = jewel;
        _jewels.Add(jewel);
    }

    private bool IsEmpty(Vector2Int index) => _area.IsCorrect(index) && _area[index.x, index.y] == null;
    private bool IsCorrect(Vector2Int index) => _area.IsCorrect(index);
}
