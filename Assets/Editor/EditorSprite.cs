using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VN.Sprite))]
public class EditorSprite : Editor
{
    public override void OnInspectorGUI()
    {
        VN.Sprite sprite = (VN.Sprite)target;
        if (sprite == null || sprite.Texture == null)
            return;

        // editable fields
        EditorGUILayout.LabelField("Texture", sprite.Texture == null ? "null" : sprite.Texture.name);
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.PrefixLabel("Texture");
        sprite.Texture = (Texture2D)EditorGUILayout.ObjectField(
                sprite.Texture,
                typeof(Texture2D),
                false
            );

        EditorGUILayout.EndHorizontal();

        sprite.Dimensions = EditorGUILayout.Vector2Field("Dimensions", sprite.Dimensions);

        // previews
        float topOffset = 80;
        float margin = 20;
        float width = EditorGUIUtility.currentViewWidth - margin * 2;

        EditorGUI.DrawPreviewTexture(
                new Rect(margin, topOffset, width, width * sprite.Texture.height / sprite.Texture.width),
                sprite.Texture
            );
    }
}