using UnityEngine;
using VN;
using UnityEngine.SceneManagement;

namespace VN
{

public class Button : Node
{
    new public static Button Create(Node _Parent, Vector2 _Offset, string _ID)
    {
        Button button = Utility.LoadObject<Button>("Prefabs/UI/Buutton", _ID, _Parent);

        button.Create(_Offset);
        return button;
    }

    void OnMouseDown()
    {
        SceneManager.LoadScene("MainScene");
    }
}

}