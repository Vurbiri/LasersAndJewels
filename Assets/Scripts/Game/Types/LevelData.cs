public class LevelData 
{
    public int Level { get; }
    public LevelType TypeNext { get; }
    public int CountNext { get; }

    public LevelData(int level, LevelType typeNext, int countNext)
    {
        Level = level;
        TypeNext = typeNext;
        CountNext = countNext;
    }

    public LevelData(LevelType typeCurrent, int countCurrent)
    {
        TypeNext = typeCurrent;
        CountNext = countCurrent;
    }
}
