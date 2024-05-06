using UnityEngine;

public class LevelGeneratorOne : ALevelGenerator<bool>
{
    public LevelGeneratorOne(Vector2Int size) : base(size) { }

    public PositionsChainSimple Generate(int count, byte type, int maxDistance)
    {
        _area = new bool[_size.x, _size.y];
        _maxDistance = maxDistance;
        
        return GenerateOne(count, type) ? new(_laser, _jewels) : null;
    }


    protected override void Add(Vector2Int index)
    {
        _jewels.Add(new(index, _type));
        _area[index.x, index.y] = true;
    }

    protected override void RemoveLast()
    {
        Vector2Int index = _jewels.Pop().Index;
        _area[index.x, index.y] = false;
    }

    protected override bool IsEmpty(Vector2Int index) => IsCorrect(index) && !_area[index.x, index.y];
}
