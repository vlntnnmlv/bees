using UnityEngine;

namespace VN
{

[RequireComponent(typeof(SpriteRenderer))]
public class Image : Node
{
    #region creation

    public static Image Create(Node _Parent, Vector2 _Offset, string _ID, string _Sprite)
    {
        Image image = Utility.CreateObject<Image>(_ID, _Parent);

        image.Create(_Offset, _Sprite);
        return image;
    }

    #endregion

    #region properties

    public ImageFlipType FlipType
    {
        set
        {
            switch (value)
            {
                case ImageFlipType.NONE:
                    m_SpriteRenderer.flipX = false;
                    m_SpriteRenderer.flipY = false;
                    break;
                case ImageFlipType.HORIZONTAL:
                    m_SpriteRenderer.flipX = false;
                    m_SpriteRenderer.flipY = true;
                    break;
                case ImageFlipType.VERTICAL:
                    m_SpriteRenderer.flipX = true;
                    m_SpriteRenderer.flipY = false;
                    break;
            }
        }
    }

    #endregion

    #region attributes

    SpriteRenderer m_SpriteRenderer;

    #endregion

    #region engine methods

    void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.hideFlags = HideFlags.HideInInspector;
    }

    #endregion

    #region service methods

    void Create(Vector2 _Offset, string _Sprite)
    {
        base.Create(_Offset);

        if (_Sprite.Length != 0 && _Sprite != null)
            m_SpriteRenderer.sprite = Resources.Load<Sprite>($"Sprites/{_Sprite}");
    }

    #endregion
}

}