using System;
using UnityEngine;

public class JewelsSimple 
{
    public byte IdType => _idType;
    public Vector2Int Index => _index;

    private byte _idType;
    private Vector2Int _index;

    public event Action<JewelsSimple> EventDeactivate;

    public void Setup(Vector2Int index, byte idType)
    {
        _index = index;
        _idType = idType;
    }

    public void Deactivate() => EventDeactivate?.Invoke(this);
}
