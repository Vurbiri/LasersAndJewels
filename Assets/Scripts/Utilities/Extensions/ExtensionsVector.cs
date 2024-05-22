using UnityEngine;

public static class ExtensionsVector
{
    public static bool IsBetween(this Vector2Int self, Vector2Int a, Vector2Int b) =>
        ((self.x == a.x && self.x == b.x) && ((a.y < b.y && a.y <= self.y && self.y <= b.y) || (a.y > b.y && b.y <= self.y && self.y <= a.y))) ||
        ((self.y == a.y && self.y == b.y) && ((a.x < b.x && a.x <= self.x && self.x <= b.x) || (a.x > b.x && b.x <= self.x && self.x <= a.x)));


    public static Vector2Int NormalizeDirection(this Vector2Int self)
    {
        int length = Mathf.Abs(self.x + self.y);

        return new(self.x /= length, self.y /= length);
    }

    public static void Turn90Right(this ref Vector2Int self)
    {
        int x = self.x;

        self.x = self.y;
        self.y = -x;
    }
    public static void Turn90Left(this ref Vector2Int self)
    {
        int x = self.x;

        self.x = -self.y;
        self.y = x;
    }

    public static Vector3 ToVector3(this Vector2Int self) => new(self.x, self.y, 0f);
}
