using System.Collections.Generic;
using UnityEngine;
using System;

namespace VN
{

public class Controller2D : MonoBehaviour
{
    static Dictionary<string, bool> m_Map = new Dictionary<string, bool>()
    {
        {"w",     false },
        {"a",     false },
        {"s",     false },
        {"d",     false },
    };

    public bool W    => m_Map["w"];
    public bool A    => m_Map["a"];
    public bool S    => m_Map["s"];
    public bool D    => m_Map["d"];
    public Action<bool>          OnChosen { get; set; }

    [SerializeField] bool m_Chosen;

    public bool Chosen
    {
        get => m_Chosen;
        set
        {
            m_Chosen = value;
            OnChosen?.Invoke(m_Chosen);
        }
    }

    public void Update()
    {
        m_Map["w"]     = Input.GetKey("w");
        m_Map["a"]     = Input.GetKey("a");
        m_Map["s"]     = Input.GetKey("s");
        m_Map["d"]     = Input.GetKey("d");
    }

    public Vector2 DefaultDirectionVector
    {
        get
        {
            Vector2 vector = Vector2.zero;

            if (W)
                vector += Vector2.up;
            if (A)
                vector += Vector2.left;
            if (S)
                vector += Vector2.down;
            if (D)
                vector += Vector2.right;

            return vector.normalized;
        }
    }
}

}