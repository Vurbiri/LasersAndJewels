using System;
using System.Collections.Generic;

public static class ExtensionsCollection
{
    public static T Pop<T>(this List<T> self)
    {
        T obj = self[^1]; self.RemoveAt(self.Count - 1);
        return obj;
    }

    public static bool Contains<T>(this Queue<T> self, T other, Func<T, T, bool> comparison)
    {
        foreach (T item in self)
            if (comparison(item, other))
                return true;

        return false;
    }
}
