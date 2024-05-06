using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorsPool : MonoBehaviour
{
    [Space]
    [SerializeField] private Transform _repository;
    [SerializeField] private Transform _container;
    [Space]
    [SerializeField] private Jewel _prefabJewel;
    [SerializeField] private int _sizePoolJewel = 50;
    [Space]
    [SerializeField] private JewelEnd _prefabJewelEnd;
    [SerializeField] private int _sizePoolJewelEnd = 2;
    [Space]
    [SerializeField] private Laser _prefabLaser;
    [SerializeField] private int _sizePoolLaser = 2;
    

    private PoolJewels _poolJewel;
    //private PoolJewelsSimple _poolJewelSimple;
    private Pool<JewelEnd> _poolJewelEnd;
    private Pool<Laser> _poolLaser;

    public void Initialize(Action onSelected)
    {
        _poolJewel = new(_prefabJewel, _repository, _sizePoolJewel, onSelected);
        //_poolJewelSimple = new(_sizePoolJewel);
        _poolJewelEnd = new(_prefabJewelEnd, _repository, _sizePoolJewelEnd);
        _poolLaser = new(_prefabLaser, _repository, _sizePoolLaser);
    }

    public Jewel GetJewel(JewelSimple jewelSimple, int count)
    {
        Jewel jewel = _poolJewel.GetObject(_container);
        jewel.Setup(jewelSimple, count);
        return jewel;
    }

    //public JewelSimple GetJewelsSimple(Vector2Int index, byte idType) => _poolJewelSimple.GetJewelsSimple(index, idType);

    public JewelEnd GetJewelEnd(JewelSimple jewelSimple)
    {
        JewelEnd jewel = _poolJewelEnd.GetObject(_container);
        jewel.Setup(jewelSimple);
        return jewel;
    }

    public Laser GetLaser(LaserSimple laserSimple)
    {
        Laser laser = _poolLaser.GetObject(_container);
        laser.Setup(laserSimple);
        return laser;
    }

    #region Nested Classe
    //***********************************
    private class PoolJewels : Pool<Jewel>
    {
        private readonly Action _onSelected;

        public PoolJewels(Jewel prefab, Transform repository, int size, Action onSelected) : base(prefab, repository, 0)
        {
            _onSelected = onSelected;
            for (int i = 0; i < size; i++)
                OnDeactivate(CreateObject());
        }

        protected override Jewel CreateObject()
        {
            Jewel gameObject = base.CreateObject();
            gameObject.EventSelected += _onSelected;
            return gameObject;
        }
    }
    //***********************************
    //private class PoolJewelsSimple
    //{
    //    private readonly Stack<JewelSimple> _pool;

    //    public PoolJewelsSimple(int size)
    //    {
    //        _pool = new(size);
    //        for (int i = 0; i < size; i++)
    //            _pool.Push(CreateObject());
    //    }

    //    public JewelSimple GetJewelsSimple(Vector2Int index, byte idType)
    //    {
    //        JewelSimple JewelsSimple;
    //        if (_pool.Count == 0)
    //            JewelsSimple = CreateObject();
    //        else
    //            JewelsSimple = _pool.Pop();

    //        JewelsSimple.Setup(index, idType);

    //        return JewelsSimple;
    //    }

    //    private JewelSimple CreateObject()
    //    {
    //        JewelSimple jewelsSimple = new();
    //        jewelsSimple.EventDeactivate += (j) => _pool.Push(j);
    //        return jewelsSimple;
    //    }
    //}
    #endregion
}
