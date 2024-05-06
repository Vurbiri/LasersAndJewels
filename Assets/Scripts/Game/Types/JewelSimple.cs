using UnityEngine;

public class JewelSimple 
{
    public byte IdType => _idType;
    public Vector2Int Index => _index;
    public int X => _index.x;
    public int Y => _index.y;

    private readonly byte _idType;
    private Vector2Int _index;

    public JewelSimple(Vector2Int index, byte idType)
    {
        _index = index;
        _idType = idType;
    }

    //public static implicit operator Vector2Int(JewelSimple obj) => obj._index;
    //public static implicit operator byte(JewelSimple obj) => obj._idType;

    public static Vector2Int operator -(JewelSimple a, JewelSimple b) => a._index - b._index;
    public static Vector2Int operator +(JewelSimple a, JewelSimple b) => a._index + b._index;

    //public override int GetHashCode() => _idType.GetHashCode();
    //public static bool operator ==(JewelSimple a, JewelSimple b) => a._value == b._value;
    //public static bool operator !=(JewelSimple a, JewelSimple b) => a._value != b._value;
}
