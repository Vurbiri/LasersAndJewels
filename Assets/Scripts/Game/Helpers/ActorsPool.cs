using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

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
    [Space]
    [SerializeField] private JewelTwoToOne _prefabJewelTwoToOne;

    private PoolJewels _poolJewel;
    private Pool<JewelEnd> _poolJewelEnd;
    private Pool<Laser> _poolLaser;
    private PoolJewelTwoToOne _poolJewelTwoToOne;

    public void Initialize(Action onSelected)
    {
        _poolJewel = new(_prefabJewel, _repository, _sizePoolJewel, onSelected);
        _poolJewelEnd = new(_prefabJewelEnd, _repository, _sizePoolJewelEnd);
        _poolLaser = new(_prefabLaser, _repository, _sizePoolLaser);
        _poolJewelTwoToOne = new(_prefabJewelTwoToOne, _repository, onSelected);
    }

    public Jewel GetJewel(Vector2Int index, int idType, int count, int group)
    {
        Jewel jewel = _poolJewel.GetObject(_container);
        jewel.Setup(index, idType, count, group);
        return jewel;
    }

    public JewelEnd GetJewelEnd(Vector2Int index, int idType)
    {
        JewelEnd jewel = _poolJewelEnd.GetObject(_container);
        jewel.Setup(index, idType);
        return jewel;
    }

    public Laser GetLaser(LaserSimple laserSimple, int idType, int maxCountRay)
    {
        Laser laser = _poolLaser.GetObject(_container);
        laser.Setup(laserSimple, idType, maxCountRay);
        return laser;
    }

    public JewelTwoToOne GetJewelTwoToOne(Vector2Int index, int idType, int maxCountRay)
    {
        JewelTwoToOne jewel = _poolJewelTwoToOne.GetObject(_container);
        jewel.Setup(index, idType, maxCountRay);
        return jewel;
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
    private class PoolJewelTwoToOne
    {
        private readonly JewelTwoToOne _gameObject;
        private readonly Transform _repository;

        public PoolJewelTwoToOne(JewelTwoToOne prefab, Transform repository, Action onSelected) 
        {
            _repository = repository;

            _gameObject = Instantiate(prefab);
            _gameObject.Initialize();
            _gameObject.EventDeactivate += OnDeactivate;
            _gameObject.EventSelected += onSelected;
            _gameObject.SetParent(_repository);

        }

        public JewelTwoToOne GetObject(Transform parent)
        {
            _gameObject.SetParent(parent);
            return _gameObject;
        }

        protected void OnDeactivate(JewelTwoToOne poolObject)
        {
            poolObject.SetParent(_repository);
        }
    }
    #endregion
}
