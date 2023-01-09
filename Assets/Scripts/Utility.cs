using UnityEngine;

namespace VN
{

public static class Utility
{
    public const float          SCREEN_MARGIN = 0.5f;

    public static float         ScreenHeight     => Camera.main.orthographicSize * 2;
    public static float         ScreenWidth      => ScreenHeight * Camera.main.aspect;
    public static Vector2       ScreenSize       => new Vector2(ScreenWidth, ScreenHeight);

    public static Vector2 RandomOffset => new Vector2(
            Random.Range(
                    -Utility.ScreenWidth/2 + Utility.SCREEN_MARGIN,
                    Utility.ScreenWidth/2 - Utility.SCREEN_MARGIN
                ),
            Random.Range(
                    -Utility.ScreenHeight/2 + Utility.SCREEN_MARGIN,
                    Utility.ScreenHeight/2 - Utility.SCREEN_MARGIN
                )
            );

    public static Vector2 RandomCenterOffset => new Vector2(
            Random.Range(-0.4f, 0.4f),
            Random.Range(-0.4f, 0.4f)
        );

    public static Vector2 TopLeftCornerOffset => new Vector2(-ScreenWidth/2 + SCREEN_MARGIN, ScreenHeight/2 - SCREEN_MARGIN);

    public static Vector2 MousePositionWorld => (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

    public static bool IsOffsetBesideScreen(Vector2 _Offset)
    {
        if (Mathf.Abs(_Offset.x) > ScreenWidth/2 - SCREEN_MARGIN)
            return true;
        if (Mathf.Abs(_Offset.y) > ScreenHeight/2 - SCREEN_MARGIN)
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
