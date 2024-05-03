using System;
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
    [SerializeField] private JewelStart _prefabJewelStart;
    [SerializeField] private int _sizePoolJewelStart = 2;
    [Space]
    [SerializeField] private JewelEnd _prefabJewelEnd;
    [SerializeField] private int _sizePoolJewelEnd = 2;

    private PoolJewels _poolJewel;
    private Pool<JewelStart> _poolJewelStart;
    private Pool<JewelEnd> _poolJewelEnd;

    public void Initialize(float sizeJewels, Action onSelected)
    {
        Jewel.Size = sizeJewels;
        JewelStart.Size = sizeJewels;
        JewelEnd.Size = sizeJewels;

        _poolJewel = new(_prefabJewel, _repository, _sizePoolJewel, onSelected);
        _poolJewelStart = new(_prefabJewelStart, _repository, _sizePoolJewelStart);
        _poolJewelEnd = new(_prefabJewelEnd, _repository, _sizePoolJewelEnd);
    }

    public Jewel GetJewel(Vector2Int index, int count)
    {
        Jewel jewel = _poolJewel.GetObject(_container);
        jewel.Setup(index, count);
        return jewel;
    }

    public JewelStart GetJewelStart(Vector2Int index, Vector2Int orientation)
    {
        JewelStart jewel = _poolJewelStart.GetObject(_container);
        jewel.Setup(index, orientation);
        return jewel;
    }

    public JewelEnd GetJewelEnd(Vector2Int index)
    {
        JewelEnd jewel = _poolJewelEnd.GetObject(_container);
        jewel.Setup(index);
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
    #endregion
}
