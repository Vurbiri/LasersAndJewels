using UnityEngine;

public class GlobalColors : ASingleton<GlobalColors>
{
    [Space]
    [SerializeField] private Color _colorDefault = new(0.87f, 0.87f, 1f , 1f);
    [SerializeField] private Color[] _colors;
    [Space]
    [SerializeField] private int _chanceColorDefaultForOne = 10;

    private readonly Color[] _colorsRandom = new Color[4];
    private readonly int[] _indexes = new int[3];
    private int _count;

    public Color this[int i] => _colorsRandom[i];

    protected override void Awake()
    {
        _isNotDestroying = false;
        base.Awake();

        _count = _colors.Length;
    }

    public void GenerateOne()
    {
        _colorsRandom[0] = URandom.IsTrue(_chanceColorDefaultForOne) ? _colorDefault : _colors.Rand();
    }

    public void GenerateTwo()
    {
        RandomRangeColors(2);
    }

    public void GenerateThree()
    {
        RandomRangeColors(3);
    }

    private void RandomRangeColors(int count)
    {
        _colorsRandom[0] = _colorDefault;

        for (int i = 1, k = 0; i <= count; i++, k++)
        {
            while (!CreateIndex(k));
            _colorsRandom[i] = _colors[_indexes[k]];
        }

        #region Local functions
        //======================
        bool CreateIndex(int max)
        {
            _indexes[max] = Random.Range(0, _count);
            for (int i = 0; i < max; i++)
                if (_indexes[max] == _indexes[i])
                    return false;
            return true;
        }
        #endregion
    }
}
