using UnityEngine;

public static class ResourceManager
{
    public static T Load<T>(string _Path, string _ID) where T : VN.Object
    {
        T obj = Object.Instantiate(Resources.Load<T>(_Path));
        obj.name = _ID;
        return obj;
    }
}