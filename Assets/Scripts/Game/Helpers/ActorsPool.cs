using System;
using UnityEngine;

public class ActorsPool : MonoBehaviour
{
    [Space]
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
    [SerializeField] private JewelOneToTwo _prefabJewelOneToTwo;

    private PoolJewels _poolJewel;
    private Pool<JewelEnd> _poolJewelEnd;
    private Pool<Laser> _poolLaser;
    private PoolJewelTo<JewelTwoToOne> _poolJewelTwoToOne;
    private PoolJewelTo<JewelOneToTwo> _poolJewelOneToTwo;

    public void Initialize(Action onSelected)
    {
        _poolJewel = new(_prefabJewel, _container, _sizePoolJewel, onSelected);
        _poolJewelEnd = new(_prefabJewelEnd, _container, _sizePoolJewelEnd);
        _poolLaser = new(_prefabLaser, _container, _sizePoolLaser);
        _poolJewelTwoToOne = new(_prefabJewelTwoToOne, _container, onSelected);
        _poolJewelOneToTwo = new(_prefabJewelOneToTwo, _container, onSelected);
    }

    public Jewel GetJewel(Vector2Int index, int idType, int count, int group)
    {
        Jewel jewel = _poolJewel.GetObject();
        jewel.Setup(index, idType, count, group);
        return jewel;
    }

    public JewelEnd GetJewelEnd(Vector2Int index, int idType)
    {
        JewelEnd jewel = _poolJewelEnd.GetObject();
        jewel.Setup(index, idType);
        return jewel;
    }

    public Laser GetLaser(LaserSimple laserSimple, int idType, int maxCountRay)
    {
        Laser laser = _poolLaser.GetObject();
        laser.Setup(laserSimple, idType, maxCountRay);
        return laser;
    }

    public JewelTwoToOne GetJewelTwoToOne(BranchData data, int typeOut, int typeInA, int typeInB, int maxCountRay)
    {
        JewelTwoToOne jewel = _poolJewelTwoToOne.GetObject();
        jewel.Setup(data, typeOut, typeInA, typeInB, maxCountRay);
        return jewel;
    }

    public JewelOneToTwo GetJewelOneToTwo(BranchData data, int typeOutA, int typeOutB, int typeIn, int maxCountRay)
    {
        JewelOneToTwo jewel = _poolJewelOneToTwo.GetObject();
        jewel.Setup(data, typeOutA, typeOutB, typeIn, maxCountRay);
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
                _pool.Push(CreateObject());
        }

        protected override Jewel CreateObject()
        {
            Jewel gameObject = base.CreateObject();
            gameObject.EventSelected += _onSelected;
            return gameObject;
        }
    }
    //***********************************
    private class PoolJewelTo<T> where T : AJewelTo
    {
        private readonly T _gameObject;

        public PoolJewelTo(T prefab, Transform repository, Action onSelected) 
        {
            _gameObject = Instantiate(prefab, repository);
            _gameObject.Initialize();
            _gameObject.EventSelected += onSelected;
        }

        public T GetObject() => _gameObject;
    }
    #endregion
}
