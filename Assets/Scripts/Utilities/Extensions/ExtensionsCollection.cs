using System;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionsCollection
{
    public static T Pop<T>(this List<T> self)
    {
        T obj = self[^1]; self.RemoveAt(self.Count - 1);
        return obj;
    }

    public static bool IsCorrect<T>(this T[,] self, Vector2Int index) => index.x >= 0 && index.x < self.GetLength(0) && index.y >= 0 && index.y < self.GetLength(1);
}
