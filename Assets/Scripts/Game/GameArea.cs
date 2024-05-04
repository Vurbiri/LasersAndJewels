using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ActorsPool))]
public class GameArea : MonoBehaviour
{
    [SerializeField] private Vector2Int _size = new(10, 9);
    [Space]
    [SerializeField] private LineRenderer _laserRay;

    private ActorsPool _jewelsPool;
    private LevelGenerator _generator;
    private JewelsArea _area;

    private JewelStart _jewelStart;
    private List<IJewel> _jewels = new();

    private int _countJewel = 23;

    private void Awake()
    {
        _jewelsPool = GetComponent<ActorsPool>();
        _jewelsPool.Initialize(OnSelected);

        _generator = new(_size);
        _area = new(_size);

        #region Local function
        //======================

        #endregion
    }

    private void Start()
    {
        PositionsChain chain;
        int attempts = 0;
        do
        {
            chain = _generator.Simple(_countJewel, 5);
            attempts++;
        }
        while (!chain.IsBuilding);

        _jewels = new(_countJewel);
        Debug.Log(attempts + "\n============================");
        Create(chain);
    }

    private void OnSelected()
    {
        foreach (IJewel jewel in _jewels)
            jewel.Off();

        JewelsChain chain = _area.Chain(_jewelStart.Orientation);

        Debug.Log(chain.IsLast);
        if (_countJewel == chain.Count)
            Debug.Log("-=/ Level complete \\=-");

        int count = chain.Count + (chain.IsLast ? 2 : 1);
        _laserRay.positionCount = count;
        Vector3[] positions = new Vector3[count];

        positions[0] = _jewelStart.LocalPosition;
        count = 1;
        foreach (IJewel jewel in chain)
        {
            jewel.On();
            positions[count++] = jewel.LocalPosition;
        }
        if (chain.IsLast) 
            positions[count] = chain.Last;

        _laserRay.SetPositions(positions);
    }
    public void Create(PositionsChain jewelsChain)
    {
        int count = 0;

        _area.Start = jewelsChain.Positions[0];
        _jewelStart = _jewelsPool.GetJewelStart(jewelsChain.Start, jewelsChain.StartOrientation);
        //_jewelStart.On();

        foreach (Vector2Int index in jewelsChain.Positions)
            Add(_jewelsPool.GetJewel(index, count++));

        Add(_jewelsPool.GetJewelEnd(jewelsChain.End));

        OnSelected();

        #region Local function
        //======================
        void Add(IJewel jewel)
        {
            _area.Add(jewel);
            _jewels.Add(jewel);
        }
        #endregion
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
