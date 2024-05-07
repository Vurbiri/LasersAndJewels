using System;
using System.Linq;

public enum AudioType : byte
{
    Music,
    SFX,
}

public enum AvatarSize : byte
{
    Small,
    Medium,
    Large
}

public enum MessageType : byte
{
    Normal,
    Warning,
    Error,
    FatalError
}

public enum LevelType
{
    LevelOne,
    LevelTwo,
}

public enum GameMode : byte
{
    Game,
    Bonus
}

public static class ExtensionsEnum
{
    public static int ToInt<T>(this T self) where T : Enum => Convert.ToInt32(self);
    //public static T ToEnum<T>(this int self) where T : Enum => (T)Enum.ToObject(typeof(T), self);

    //public static T ToEnum<T>(this string self) where T : Enum => (T)Enum.Parse(typeof(T), self, true);
    //public static int ToEnumInt<T>(this string self) where T : Enum => self.ToEnum<T>().ToInt<T>();
}

public class Enum<T> where T : Enum
{
    public static int Count => Enum.GetNames(typeof(T)).Length;
    public static T[] GetValues() => Enum.GetValues(typeof(T)).Cast<T>().ToArray<T>();
}
