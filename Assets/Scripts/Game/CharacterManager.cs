using UnityEngine;
using System.Collections.Generic;

namespace VN
{

public class CharacterManager : MonoBehaviour
{
    #region properties
    
    public static CharacterManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<CharacterManager>();

            if (m_Instance == null)
            {
                GameObject go = new GameObject("CharacterManager");
                m_Instance = go.AddComponent<CharacterManager>();
            }

            return m_Instance;
        }
    }

    public static Rect RandomGroundRect => new Rect(
            Random.Range(
                    -Utility.ScreenWidth/2 + Utility.SCREEN_MARGIN,
                    Utility.ScreenWidth/2 - Utility.SCREEN_MARGIN
                ),
            Random.Range(
                    -Utility.ScreenHeight/2 + Utility.SCREEN_MARGIN,
                    0
                ),
            1,
            1
        );

    public static Rect RandomSkyRect => new Rect(
            Random.Range(
                    -Utility.ScreenWidth/2 + Utility.SCREEN_MARGIN,
                    Utility.ScreenWidth/2 - Utility.SCREEN_MARGIN
                ),
            Random.Range(
                    0,
                    Utility.ScreenHeight/2 - Utility.SCREEN_MARGIN
                ),
            1,
            1
        );

    public static Rect RandomRect => new Rect(
            Random.Range(
                    -Utility.ScreenWidth/2 + Utility.SCREEN_MARGIN,
                    Utility.ScreenWidth/2 - Utility.SCREEN_MARGIN
                ),
            Random.Range(
                    -Utility.ScreenHeight/2 + Utility.SCREEN_MARGIN,
                    Utility.ScreenHeight/2 - Utility.SCREEN_MARGIN
                ),
            1,
            1
        );

    public static Rect RandomCenterRect => new Rect(
            Random.Range(-0.4f, 0.4f),
            Random.Range(-0.4f, 0.4f),
            1,
            1
        );

    #endregion

    #region attirbutes

    static CharacterManager m_Instance;

    Dictionary<string, CharacterConfig> m_Configs = new();

    #endregion

    #region public methods

    public T Create<T>(
            string _ID,
            Node _Parent,
            Rect _Rect
        ) where T : Character
    {
        string typeName = typeof(T).Name;
        CharacterConfig config = m_Configs.TryGetValue(typeName, out config)
            ? config
            : Resources.Load<CharacterConfig>($"Configs/{typeName}");

        if (config == null)
            Debug.Log("[CharacterManager] Couldn't load config for type " + typeName);

        T character = Character.Create<T>(_ID, _Parent, _Rect, config);
        return character as T;
    }

    #endregion

}

}