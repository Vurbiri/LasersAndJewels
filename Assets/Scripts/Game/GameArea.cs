using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(ActorsPool))]
public class GameArea : MonoBehaviour
{
    [SerializeField] private Vector2Int _size = new(10, 9);

    private ActorsPool _actorsPool;
    private LevelGenerator _generator;
    private JewelsArea _area;

    private Laser _laser;
    private List<IJewel> _jewels = new();

    private int _countJewel = 25;

    private void Awake()
    {
        _actorsPool = GetComponent<ActorsPool>();
        _actorsPool.Initialize(OnSelected);

        _generator = new(_size);
        _area = new(_size);


        #region Local function
        //======================

        #endregion
    }

    private void Start()
    {
        PositionsChain chain;
        int attempts = 0, maxAttempts = 1000;

        do
        {
            chain = _generator.Simple(_countJewel, 5);
        }
        while (!chain.IsBuilding && attempts++ < maxAttempts);

        _jewels = new(_countJewel);
        Create(chain);

        Debug.Log(attempts + "\n============================");

    }

    private void OnSelected()
    {
        JewelsChain chain = _area.Chain(_laser.Orientation);

        bool isLevelComplete = _countJewel == chain.Count;
        Vector3[] positions = new Vector3[chain.Count + (chain.IsLast ? 2 : 1)];

        positions[0] = _laser.StartPosition;
        int count = 1;
        foreach (IJewel jewel in chain)
        {
            jewel.TurnOn(isLevelComplete);
            positions[count++] = jewel.LocalPosition;
        }
        if (chain.IsLast) 
            positions[count] = chain.Last;

        _laser.SetRayPositions(positions);

        foreach (IJewel jewel in _jewels)
            jewel.TurnOff();

        if (isLevelComplete)
            Debug.Log("-=/ Level complete \\=-");
    }
    public void Create(PositionsChain jewelsChain)
    {
        int count = 0;

        _area.Start = jewelsChain.Positions[0];
        _laser = _actorsPool.GetLaser(jewelsChain.Start, jewelsChain.StartOrientation, 0);

        IJewel jewel = null;
        foreach (Vector2Int index in jewelsChain.Positions)
        {
            jewel = _actorsPool.GetJewel(index, 0, count++);
            Add(jewel);
            jewel.Run();
        }

        jewel = _actorsPool.GetJewelEnd(jewelsChain.End, 0);
        Add(jewel);
        jewel.Run();


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
