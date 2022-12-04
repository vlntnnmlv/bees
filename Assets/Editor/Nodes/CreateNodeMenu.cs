#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using VN;

[ExecuteInEditMode]
public class CreateNodeMenu
{
    [MenuItem("GameObject/Node/Empty Node", false, 1)]
    private static void CreateEmptyNode(MenuCommand menuCommand)
    {
        Node parent = Selection.activeGameObject?.GetComponent<Node>();

        Node node = Node.Create(parent, Vector2.zero, "Node");        
    }

    [MenuItem("GameObject/Node/Image", false, 2)]
    private static void CreateImage(MenuCommand menuCommand)
    {
        Node parent = Selection.activeGameObject?.GetComponent<Node>();

        Image node = Image.Create(parent, Vector2.zero, "Imahe", "");       
    }
}
#endif