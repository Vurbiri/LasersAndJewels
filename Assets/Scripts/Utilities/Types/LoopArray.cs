using System;
using System.Collections.Generic;

public class LoopArray<T>
{
    private const int CAPACITY_DEFAULT = 4;
    private T[] _array;
    private int _cursor = 0, _default = 0, _count = 0, _capacity = CAPACITY_DEFAULT;

    public int Count => _count;

    public T Default => _array[_cursor = _default];
    public T Forward
    {
        get
        {
           if(++_cursor >= _count) _cursor = 0;

            return _array[_cursor];
        }
    }
    public T Back
    {
        get
        {
            if (--_cursor < 0) _cursor = _count - 1;

            return _array[_cursor];
        }
    }

    public LoopArray() => _array = new T[_capacity];
    public LoopArray(int capacity)
    {
        if (capacity > 0)
            _capacity = capacity;

        _array = new T[_capacity];
    }

    public LoopArray(T[] array)
    {
        _capacity = _count = array.Length;
        _array = new T[_capacity];

        for(int i = 0; i < _count; i++)
            _array[i] = array[i];
    }

    public void Add(T item)
    {
        while (_count >= _capacity)
            Array.Resize<T>(ref _array, _capacity <<= 1);

        _array[_count++] = item;
    }

    public void SetDefault(int value) => _default = value;
}
