using System.Collections.Generic;
using UnityEngine;

public class JewelsAreaSimple
{
    private IJewel[,] _area;
    Vector2Int _size, _start, _laserOrientation;

    private IJewel this[Vector2Int index] { get => _area[index.x, index.y]; set => _area[index.x, index.y] = value; }

    public JewelsAreaSimple(Vector2Int size)
    {
        _area = new IJewel[size.x, size.y];
        _size = size;
    }

    public void Setup(PositionsChainSimple positionsChain)
    {
        _start = positionsChain.Jewels[0].Index;
        _laserOrientation = positionsChain.Laser.Orientation;
    }

    public void Add(IJewel jewel) => this[jewel.Index] = jewel;

    public JewelsChain Chain()
    {
        HashSet<IJewel> jewels = new();
        
        IJewel current = this[_start];
        Vector2Int direction = current.Orientation, directionOld = _laserOrientation, index = _start;

        while (jewels.Add(current) && direction != Vector2Int.zero) 
        {
            while (IsEmpty(index += direction));

            if (!IsCorrect(index)) return new(jewels, index, directionOld != -direction);

            current = this[index];
            directionOld = direction;
            direction = current.Orientation;
        }

        return new(jewels, index, direction != Vector2Int.zero && directionOld != -direction);
    }
    public void Reset() => _area = new IJewel[_size.x, _size.y];

    private bool IsEmpty(Vector2Int index) => IsCorrect(index) && _area[index.x, index.y] == null;
    private bool IsCorrect(Vector2Int index) => index.x >= 0 && index.x < _size.x && index.y >= 0 && index.y < _size.y;
}
