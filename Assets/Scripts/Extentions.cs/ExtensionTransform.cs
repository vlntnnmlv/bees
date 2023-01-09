using UnityEngine;

public static class ExtensionTransform
{
    public static Rect TransformRect(this Transform _Transform, Rect _Rect)
    {
        return new Rect(
				_Rect.x * _Transform.lossyScale.x + _Transform.position.x,
				_Rect.y * _Transform.lossyScale.y + _Transform.position.y,
				_Rect.width * _Transform.lossyScale.x,
				_Rect.height * _Transform.lossyScale.y
			);
    }
}