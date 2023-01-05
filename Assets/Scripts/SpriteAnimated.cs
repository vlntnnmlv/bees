using UnityEngine;
using System;
using System.Collections;

public class SpriteAnimated : MonoBehaviour
{

    public Action OnAnimationPlayed { get; set; }
    public bool   Playing
    {
        get => m_Playing;
        set
        {
            m_Playing = value;
            if (!m_Playing)
                CurrentSprite = 0;
        }
    }

    int CurrentSprite
    {
        get => m_CurrentSprite;
        set
        {
            m_CurrentSprite = value;
            m_CurrentSprite %= m_Sprites.Length;
            m_SpriteRenderer.sprite = m_Sprites[m_CurrentSprite];
        }
    }

    [SerializeField] float    m_FramePeriod = 0.1f;
    [SerializeField] Sprite[] m_Sprites;
    int            m_CurrentSprite = 0;
    SpriteRenderer m_SpriteRenderer;
    float          m_SpriteUpdateTime = 0;
    bool           m_Playing = true;

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Playing && Time.time - m_SpriteUpdateTime > m_FramePeriod)
        {
            ++CurrentSprite;
            m_SpriteUpdateTime = Time.time;
        }
    }
}