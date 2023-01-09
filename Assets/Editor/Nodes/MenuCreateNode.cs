#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using VN;

[ExecuteInEditMode]
public class MenuCreateNode
{
    [MenuItem("GameObject/Node/Node", false, 1)]
    private static void CreateEmptyNode(MenuCommand menuCommand)
    {
        Node parent = Selection.activeGameObject?.GetComponent<Node>();

        Node node = Node.Create("Node", parent, new Rect(0, 0, 1, 1));        
    }

    [MenuItem("GameObject/Node/Image", false, 2)]
    private static void CreateImage(MenuCommand menuCommand)
    {
        Node parent = Selection.activeGameObject?.GetComponent<Node>();

        Image node = Image.Create("Image", parent, new Rect(0, 0, 1, 1), "hive");       
    }

    [MenuItem("GameObject/Node/Text", false, 3)]
    private static void CreateText(MenuCommand menuCommand)
    {
        Node parent = Selection.activeGameObject?.GetComponent<Node>();

        Text node = Text.Create("Text", parent, new Rect(0, 0, 1, 1), "hive");       
    }
}
#endif