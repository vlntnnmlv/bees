using UnityEngine;
using System;
using System.Collections;

public class SpriteAnimated : MonoBehaviour
{

    public Action OnAnimationPlayed { get; set; }
    public bool   Playing           { get; set; } = true;

    [SerializeField] Sprite[] m_Sprites;

    int            m_CurrentSprite = 0;
    SpriteRenderer m_SpriteRenderer;

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Playing)
        {
            if (m_CurrentSprite < m_Sprites.Length)
                m_SpriteRenderer.sprite = m_Sprites[m_CurrentSprite];

            ++m_CurrentSprite;
            m_CurrentSprite %= m_Sprites.Length;
        }
    }

}