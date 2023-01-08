using UnityEngine;
using VN;

namespace VN
{

public enum AnimationType
{
    NONE = 0,
    LOOP,
    ONCE
}

public class ImageAnimated : Image
{
    #region attributes

    [Space, Header("Animation")]
    [SerializeField] AnimationType m_AnimationType;
    [SerializeField] float         m_FrameTime = 0.01f;
    bool                           m_AnimationPlayed;
    float                          m_FrameChangedTime;

    #endregion

    #region properties

    public AnimationType AnimationType
    {
        get => m_AnimationType;
        set => m_AnimationType = value;
    }

    #endregion

    #region engine methods

    protected override void Update()
    {
        base.Update();

        if (Application.isPlaying && m_Sprite != null && m_AnimationType != AnimationType.NONE && !m_AnimationPlayed && Time.time - m_FrameChangedTime > m_FrameTime)
        {
            m_SpriteID = (m_SpriteID + 1) % m_Sprite.SpritesCount;
            m_FrameChangedTime = Time.time;

            if (m_SpriteID == m_Sprite.SpritesCount - 1 && m_AnimationType == AnimationType.ONCE)
                m_AnimationPlayed = true;
        }
    }

    #endregion
}

}