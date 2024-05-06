using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LevelTwo
{
    private readonly ActorsPool _actorsPool;
    private readonly LevelGeneratorTwo _generator;
    private readonly JewelsAreaSimple _area;
    private Vector2Int _size;

    private int _countJewelOne, _countJewelTwo;
    private Laser _laserOne, _laserTwo;
    private List<IJewel> _jewelsOne, _jewelsTwo;

    public LevelTwo(Vector2Int size, ActorsPool actorsPool)
    {
        _size = size;
        _actorsPool = actorsPool;

        _generator = new(_size);
        _area = new(_size);
    }

    public bool Create(int countOne, byte typeOne, int countTwo, byte typeTwo, byte chance, int maxDistance)
    {
        Reset();

        PositionsChainSimple[] positionsChain = Generate();
        if (positionsChain == null) return false;

        _jewelsOne = new(countOne);
        _jewelsTwo = new(countTwo);
        //_area.Setup(positionsChain);

        _laserOne = _actorsPool.GetLaser(positionsChain[0].Laser);

        countOne = countTwo = 0;
        foreach (var jewel in positionsChain[0].Jewels)
            AddOne(_actorsPool.GetJewel(jewel, countOne++));

        AddOne(_actorsPool.GetJewelEnd(positionsChain[0].End));

        _laserTwo = _actorsPool.GetLaser(positionsChain[1].Laser);

        foreach (var jewel in positionsChain[1].Jewels)
            AddTwo(_actorsPool.GetJewel(jewel, countTwo++));

        AddTwo(_actorsPool.GetJewelEnd(positionsChain[1].End));

        return true;

        #region Local functions
        //======================
        void Reset()
        {
            _countJewelOne = countOne;
            _countJewelTwo = countTwo;
            if (_laserOne == null) return;

            _laserOne.Deactivate();
            _laserOne = null;
            _laserTwo.Deactivate();
            _laserTwo = null;
            _jewelsOne.ForEach((j) => j.Deactivate());
            _jewelsOne = null;
            _jewelsTwo.ForEach((j) => j.Deactivate());
            _jewelsTwo = null;
            _area.Reset();
        }
        //======================
        PositionsChainSimple[] Generate()
        {
            PositionsChainSimple[] chain;
            int attempts = 0, maxAttempts = (countOne + countTwo) << 3;

            do chain = _generator.Generate(countOne, typeOne, countTwo, typeTwo, chance, maxDistance);
            while (++attempts < maxAttempts && chain == null);

            Debug.Log("attempts: " + attempts + "/" + maxAttempts + "\n============================");

            return chain;
        }
        //======================
        void AddOne(IJewel jewel)
        {
            //_area.Add(jewel);
            _jewelsOne.Add(jewel);
        }
        //======================
        void AddTwo(IJewel jewel)
        {
            //_area.Add(jewel);
            _jewelsTwo.Add(jewel);
        }
        #endregion
    }

    public void Run()
    {
        _jewelsOne.ForEach((j) => j.Run());
        _jewelsTwo.ForEach((j) => j.Run());
        //Check();
    }
}
