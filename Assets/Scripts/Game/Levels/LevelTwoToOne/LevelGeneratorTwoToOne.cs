using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorTwoToOne : ALevelGeneratorTwo
{
    private int _offset;

    public LevelGeneratorTwoToOne(Vector2Int size) : base(size) { }

    public PositionsChainTwo Generate(int countOne, int typeOne, int countTwo, int typeTwo, int chance, int maxDistance)
    {
        _area = new bool[_size.x, _size.y];
        _maxDistance = maxDistance;
        _chance = chance;
        _offset = (countTwo >> 1) + 1;

        _typeBase = _typeCurrent = typeOne;
        SetupOne(countOne);
        if (!GenerateBase()) return null;

        _typeBase = _typeCurrent = typeTwo;
        if (!SetupTwo(countTwo - 1) || !GenerateBase()) return null;

        _maxDistance <<= 2;
        int connect = Connect();
        if (connect < _offset) return null;

        Add();

        return new(_laserOne, _jewelsOne, _laserCurrent, _jewelsCurrent);


        #region Local functions
        //======================
        int Connect()
        {
            Vector2Int connectCurrent, connectOne, direction;
            int count = _jewelsOne.Count - _offset;
            while (_jewelsCurrent.Count > _offset)
            {
                connectCurrent = _jewelsCurrent[^1].Index;

                for (int i = _offset; i < count; i++)
                {
                    connectOne = _jewelsOne[i].Index;
                    if (IsConnect())
                        return i;
                }

                RemoveLast();
            }

            return -1;

            #region Local functions
            //======================
            bool IsConnect()
            {
                if (connectOne.x == connectCurrent.x || connectOne.y == connectCurrent.y)
                    return Insert();

                return Cross(ref connectOne, ref connectCurrent) || Cross(ref connectCurrent, ref connectOne);

            }
            //======================
            bool Insert()
            {
                if (!IsSpace(connectCurrent, connectOne))
                    return false;

                _indexCurrent = URandom.Vector2Int(connectCurrent, connectOne);
                if (funcIsNotBetween())
                    return true;

                Vector2Int temp = _indexCurrent;

                while ((_indexCurrent += direction) != connectCurrent)
                    if (funcIsNotBetween())
                        return true;

                _indexCurrent = temp;

                while ((_indexCurrent -= direction) != connectOne)
                    if (funcIsNotBetween())
                        return true;

                return false;
            }
            //======================
            bool Cross(ref Vector2Int a, ref Vector2Int b)
            {
                _indexCurrent = new(a.x, b.y);
                if (!funcIsNotBetween())
                    return false;

                return IsSpace(_indexCurrent, a) && IsSpace(_indexCurrent, b);
            }
            //======================
            bool IsSpace(Vector2Int a, Vector2Int b)
            {
                direction = (a - b).NormalizeDirection();
                a -= direction;
                while (a != b)
                {
                    if (!IsEmptySimple(b += direction))
                        return false;
                }

                return true;
            }
            //======================
            bool IsEmptySimple(Vector2Int index) => !_area[index.x, index.y];
            #endregion
        }
        #endregion
    }
}
