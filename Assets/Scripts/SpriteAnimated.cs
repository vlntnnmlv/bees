using UnityEngine;
using System;
using System.Collections;

public class SpriteAnimated : MonoBehaviour
{

    public Action    OnAnimationPlayed { get; set; }

    [SerializeField] Sprite[] m_Sprites;

    int            m_CurrentSprite = 0;
    SpriteRenderer m_SpriteRenderer;
    bool           m_Playing;

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
            if (m_CurrentSprite < m_Sprites.Length)
                m_SpriteRenderer.sprite = m_Sprites[m_CurrentSprite];

            ++m_CurrentSprite;
            m_CurrentSprite %= m_Sprites.Length;
    }

}