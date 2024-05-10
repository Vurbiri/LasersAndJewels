using System.Collections.Generic;
using UnityEngine;

public abstract class ALevel
{ 
    protected ActorsPool _actorsPool;
    protected Vector2Int _size;

    protected IJewel[,] _area;
    protected List<IJewel> _jewels;
    protected Laser _laserOne;
    protected int _count;

    protected IJewel this[Vector2Int index] { get => _area[index.x, index.y]; set => _area[index.x, index.y] = value; }

    public abstract LevelType Type { get; }

    protected const int SHIFT_ATTEMPS = 4;
    protected const int TYPE_ONE = 1, TYPE_TWO = 2, TYPE_THREE = 3;

    public ALevel(Vector2Int size, ActorsPool actorsPool)
    {
        _size = size;
        _area = new IJewel[size.x, size.y];
        _actorsPool = actorsPool;
    }

    //public virtual void Initialize(Vector2Int size, ActorsPool actorsPool)
    //{
    //    _size = size;
    //    _area = new IJewel[size.x, size.y];
    //    _actorsPool = actorsPool;
    //}

    public abstract bool Create(int count, int maxDistance);

    public virtual void Run()
    {
        _laserOne.Run();
        _jewels.ForEach((j) => j.Run());
        CheckChain();
    }

    public abstract bool CheckChain();

    public virtual void Clear()
    {
        _area = new IJewel[_size.x, _size.y];
        _laserOne.Deactivate();
        _laserOne = null;
        _jewels.ForEach((j) => j.Deactivate());
        _jewels = null;
    }

    protected void Add(IJewel jewel)
    {
        this[jewel.Index] = jewel;
        _jewels.Add(jewel);
    }

    protected bool IsEmpty(Vector2Int index) => _area.IsCorrect(index) && _area[index.x, index.y] == null;
    protected bool IsCorrect(Vector2Int index) => _area.IsCorrect(index);
}
