using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelsArea
{
    private readonly IJewel[,] _area;
    Vector2Int _size;

    public Vector2Int Start { get; set; }

    private IJewel this[Vector2Int index] { get => _area[index.x, index.y]; set => _area[index.x, index.y] = value; }

    public JewelsArea(Vector2Int size)
    {
        _area = new IJewel[size.x, size.y];
        _size = size;
    }

    public void Add(IJewel jewel) => this[jewel.Index] = jewel;

    public JewelsChain Chain(Vector2Int directionOld)
    {
        HashSet<IJewel> jewels = new();
        
        IJewel current = this[Start];
        Vector2Int direction = current.Orientation, index = Start;

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


    private bool IsEmpty(Vector2Int index) => IsCorrect(index) && _area[index.x, index.y] == null;
    private bool IsCorrect(Vector2Int index) => index.x >= 0 && index.x < _size.x && index.y >= 0 && index.y < _size.y;
}
