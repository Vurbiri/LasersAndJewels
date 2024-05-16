using UnityEngine;

public class GlobalColors : ASingleton<GlobalColors>
{
    [Space]
    [SerializeField] private Color _colorDefault = new(0.894f, 0.894f, 0.957f, 1f);
    [SerializeField] private Color[] _colors;

    private readonly Color[] _colorsRandom = new Color[4];
    private readonly int[] _indexes = new int[3];
    private int _count, _chanceColorDefaultForOne;

    public Color this[int i] => _colorsRandom[i];

    protected override void Awake()
    {
        _isNotDestroying = false;
        base.Awake();

        _count = _colors.Length;
        _chanceColorDefaultForOne = 100 / (_count + 1);

        _colorsRandom[0] = _colorDefault;
    }

    public void GenerateOne()
    {
        _colorsRandom[1] = URandom.IsTrue(_chanceColorDefaultForOne) ? _colorDefault : _colors.Rand();
        //_colorsRandom[1] = _colors[1];
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
