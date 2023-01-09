using UnityEngine;

public static class ExtensionRect
{
    public static bool Intersects(this Rect _RectA, Rect _RectB)
    {
        Vector2 centerDiff = _RectA.center - _RectB.center;
        
        if (centerDiff.x < 0)
            centerDiff.x *= -1;
        if (centerDiff.y < 0)
            centerDiff.y *= -1;

        return (
            centerDiff.x < _RectA.width  / 2 + _RectB.width  / 2 &&
            centerDiff.y < _RectA.height / 2 + _RectB.height / 2
        );
    }
}