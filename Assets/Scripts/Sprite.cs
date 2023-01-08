using UnityEngine;
using System;
namespace VN
{

[CreateAssetMenu(fileName = "Texture", menuName = "ScriptableObjects/SpawnMyTexture", order = 1)]
[Serializable]
public class Sprite : ScriptableObject
{
    #region attributes

    public Texture2D Texture;
    public Vector2   Dimensions;

    Vector2[] m_UV;

    #endregion

    #region properties

    public UnityEngine.Sprite SpriteInternal
    {
        get
        {
            UnityEngine.Sprite sprite = UnityEngine.Sprite.Create(Texture, new Rect(0, 0, Texture.width, Texture.height), new Vector2(0.5f, 0.5f));
            sprite.name = "particle";
            return sprite;
        }
    }

    public int SpritesCount   => (int)(Dimensions.x * Dimensions.y);

    #endregion

    #region public methods

    public Vector2[] CalculateUV(int _ID)
    {
        if (_ID < 0 || _ID >= SpritesCount)
        {
            Debug.LogWarning("[Sprite] Sprite ID out of range, clamping to the appropriate range.");
            _ID = Mathf.Clamp(_ID, 0, SpritesCount - 1);
        }

        float relWidthInTiles  = 1 / Dimensions.x;
        float relHeightInTiles = 1 / Dimensions.y;

        int x = (int)(_ID % Dimensions.x);
        int y = _ID / (int)Dimensions.x;

        Vector2[] uvs = new Vector2[4]
        {
            new Vector2( x      * relWidthInTiles, (Dimensions.y - y - 1) * relHeightInTiles),
            new Vector2((x + 1) * relWidthInTiles, (Dimensions.y - y - 1) * relHeightInTiles),
            new Vector2((x + 1) * relWidthInTiles, (Dimensions.y - y)     * relHeightInTiles),
            new Vector2( x      * relWidthInTiles, (Dimensions.y - y)     * relHeightInTiles),
        };

        m_UV = uvs;

        return uvs;
    }

    #endregion
}

}