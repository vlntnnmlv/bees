using UnityEngine;

public class SpriteAnimated : MonoBehaviour
{
    [SerializeField] Sprite[] m_Sprites;

    int m_CurrentSprite = 0;
    SpriteRenderer m_SpriteRenderer;

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (m_CurrentSprite < m_Sprites.Length)
            m_SpriteRenderer.sprite = m_Sprites[m_CurrentSprite];

        m_CurrentSprite += 1;
        m_CurrentSprite %= m_Sprites.Length;
    }
}
    