using UnityEngine;

namespace VN
{

public static class Utility
{
    public static float         Height => Camera.main.orthographicSize * 2;
    public static float         Width  => Height * Camera.main.aspect;
    public static Vector2 RandomOffset => new Vector2(Random.Range(-Width/2, Width/2), Random.Range(-Height/2, Height/2));
}

}
