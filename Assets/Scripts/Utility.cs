using UnityEngine;

namespace VN
{

public static class Utility
{
    public static float         Height       => Camera.main.orthographicSize * 2;
    public static float         Width        => Height * Camera.main.aspect;
    public static Vector2 RandomOffset       => new Vector2(Random.Range(-Width/2, Width/2), Random.Range(-Height/2, Height/2));
    public static Vector2 MousePositionWorld => (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

    public static T Load<T>(string _Path) where T : Object
    {
        T obj = Object.Instantiate(Resources.Load<T>(_Path));
        return obj;
    }

    public static T LoadObject<T>(string _Path, string _ID, Node _Parent = null) where T : Node
    {
        T obj = Object.Instantiate(Resources.Load<T>(_Path));

        obj.name = _ID;
        obj.SetParent(_Parent);

        return obj;
    }

    public static T CreateObject<T>(string _ID, Node _Parent = null) where T : Node
    {
        T obj = new GameObject().AddComponent<T>();

        obj.name = _ID;
        obj.SetParent(_Parent);

        return obj;
    }
}

}
