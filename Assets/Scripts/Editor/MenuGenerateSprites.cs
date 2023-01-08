using UnityEngine;
using UnityEditor;
using System.IO;

public class MenuGenerateSprites : MonoBehaviour
{
    const string TEXTURES_PATH = "Assets/Resources/Textures/";
    const string SPRITES_PATH  = "Assets/Resources/Sprites/";

    [MenuItem("VN/Generate Sprites")]
    public static void ShowWindow()
    {
        Debug.Log("[MenuGenerateSprites] Generating sprites...");
        Generate();
    }

    private static void Generate()
    {
        Texture2D[] textures = Resources.LoadAll<Texture2D>("Textures");
        int spritesCreatedCount = 0;

        foreach (Texture2D texture in textures)
        {
            if (File.Exists(SPRITES_PATH + texture.name + ".asset"))
                continue;

            VN.Sprite sprite = ScriptableObject.CreateInstance<VN.Sprite>();
            spritesCreatedCount++;
            sprite.Texture = texture;
            sprite.Dimensions = Vector2.one;
            AssetDatabase.CreateAsset(sprite, SPRITES_PATH + texture.name + ".asset");
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"[MenuGenerateSprites] Sprites created: {spritesCreatedCount}");
    }
}