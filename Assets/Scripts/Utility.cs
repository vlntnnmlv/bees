using UnityEngine;

namespace VN
{

public static class Utility
{
    const float SCREEN_MARGIN = 0.5f;
    public static float         Height     => Camera.main.orthographicSize * 2;
    public static float         Width      => Height * Camera.main.aspect;
    public static Vector2       Size       => new Vector2(Width, Height);

    public static Vector2       RandomGroundOffset => new Vector2(
        Random.Range(
                -Width/2 + SCREEN_MARGIN,
                Width/2 - SCREEN_MARGIN
            ),
        Random.Range(
                -Height/2 + SCREEN_MARGIN,
                0
            )
        );
    public static Vector2       RandomSkyOffset => new Vector2(
        Random.Range(
                -Width/2 + SCREEN_MARGIN,
                Width/2 - SCREEN_MARGIN
            ),
        Random.Range(
                0,
                Height/2 - SCREEN_MARGIN
            )
        );

    public static Vector2       RandomOffset => new Vector2(
        Random.Range(
                -Width/2 + SCREEN_MARGIN,
                Width/2 - SCREEN_MARGIN
            ),
        Random.Range(
                -Height/2 + SCREEN_MARGIN,
                Height/2 - SCREEN_MARGIN
            )
        );

    public static Vector2 RandomCenterOffset => Vector2.one * Random.Range(-0.4f, 0.4f);

    public static Vector2 TopLeftCornerOffset => new Vector2(-Width/2 + SCREEN_MARGIN, Height/2 - SCREEN_MARGIN);

    public static Vector2 MousePositionWorld => (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

    public static bool IsOffsetBesideScreen(Vector2 _Offset)
    {
        if (Mathf.Abs(_Offset.x) > Width/2 - SCREEN_MARGIN)
            return true;
        if (Mathf.Abs(_Offset.y) > Height/2 - SCREEN_MARGIN)
            return true;

        return false;
    }


    public static T Load<T>(string _Path) where T : Object
    {
        T res = Resources.Load<T>(_Path);

        if (res == null)
        {
            Debug.Log("Resource not found: " + _Path);
            return null;
        }

        T obj = Object.Instantiate(res);
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
