using UnityEngine;
using VN;
using UnityEngine.SceneManagement;
using System;

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

    public Action OnClicked { get; set; }

    void OnMouseDown()
    {
        StartCoroutine(Coroutines.Update(
                null,
                _Phase => LocalScale = Vector2.one * (1 - _Phase),
                () => SceneManager.LoadScene("MainScene"),
                0.3f
            )
        );
    }
}

}