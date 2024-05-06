using System.Collections.Generic;
using UnityEngine;

public class LevelOne
{
    private readonly ActorsPool _actorsPool;
    private readonly LevelGeneratorOne _generator;
    private readonly JewelsAreaSimple _area;
    private Vector2Int _size;

    private int _countJewel;
    private Laser _laser;
    private List<IJewel> _jewels;

    public LevelOne(Vector2Int size, ActorsPool actorsPool)
    {
        _size = size;
        _actorsPool = actorsPool;

        _generator = new(_size);
        _area = new(_size);
    }

    public bool Create(int count, byte type, int maxDistance)
    {
        Reset();

        PositionsChainSimple positionsChain = Generate();
        if (positionsChain == null) return false;

        _jewels = new(count);
        _area.Setup(positionsChain);
        
        _laser = _actorsPool.GetLaser(positionsChain.Laser);

        count = 0;
        foreach (var jewel in positionsChain.Jewels)
            Add(_actorsPool.GetJewel(jewel, count++));

        Add(_actorsPool.GetJewelEnd(positionsChain.End));

        return true;

        #region Local functions
        //======================
        void Reset()
        {
            _countJewel = count;
            if (_laser == null) return;

            _laser.Deactivate();
            _laser = null;
            _jewels.ForEach((j) => j.Deactivate());
            _jewels = null;
            _area.Reset();
        }
        //======================
        PositionsChainSimple Generate()
        {
            PositionsChainSimple chain;
            int attempts = 0, maxAttempts = count << 3;

            do chain = _generator.Generate(count, type, maxDistance);
            while (++attempts < maxAttempts && chain == null);

            Debug.Log("attempts: " + attempts + "/" + maxAttempts + "\n============================");

            return chain;
        }
        //======================
        void Add(IJewel jewel)
        {
            _area.Add(jewel);
            _jewels.Add(jewel);
        }
        #endregion
    }

    public void Run()
    {
        _jewels.ForEach((j) => j.Run());
        Check();
    }

    public bool Check()
    {
        JewelsChain chain = _area.Chain();

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

        return isLevelComplete;
       
    }

}
