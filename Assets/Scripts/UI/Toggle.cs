using UnityEngine;
using System.Collections;
using System;
using VN;

namespace VN
{

public class Toggle : Node
{

    [SerializeField] Node m_Button;

    public Action<bool> OnToggled { get; set; }
    bool   Toggled { get; set; }

    void OnMouseDown()
    {
        StartCoroutine(ToggleCoroutine());
    }

    IEnumerator ToggleCoroutine()
    {
        Vector2 from = m_Button.Offset;
        Vector2 to   = from + (Toggled ? Vector2.left : Vector2.right) * 0.5f; 

        yield return StartCoroutine(Coroutines.Update(
                null,
                _Phase => m_Button.Offset = Vector2.Lerp(from, to, _Phase),
                () => Toggled = !Toggled,
                0.3f
            )
        );
    }

}

}