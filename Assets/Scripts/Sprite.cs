using UnityEngine;

namespace VN
{

[CreateAssetMenu(fileName = "Texture", menuName = "ScriptableObjects/SpawnMyTexture", order = 1)]
public class Sprite : ScriptableObject
{
    public Texture2D Texture;
    public Vector2   Dimensions;
    public int       ID;

    public Vector2[] GetUV()
    {
        if (ID < 0 || ID >= Dimensions.x * Dimensions.y)
        {
            Debug.LogWarning("[Sprite] Sprite ID out of range, clamping to the appropriate range.");
            ID = Mathf.Clamp(ID, 0, (int)(Dimensions.x * Dimensions.y - 1));
        }

        float relWidthInTiles  = 1 / Dimensions.x;
        float relHeightInTiles = 1 / Dimensions.y;

        int x = (int)(ID % Dimensions.x);
        int y = Mathf.RoundToInt(ID / Dimensions.x);

        Vector2[] uvs = new Vector2[4]
        {
            new Vector2( x      * relWidthInTiles, (Dimensions.y - y - 1) * relHeightInTiles),
            new Vector2((x + 1) * relWidthInTiles, (Dimensions.y - y - 1) * relHeightInTiles),
            new Vector2((x + 1) * relWidthInTiles, (Dimensions.y - y)     * relHeightInTiles),
            new Vector2( x      * relWidthInTiles, (Dimensions.y - y)     * relHeightInTiles),
        };

        return uvs;
    }
}

}