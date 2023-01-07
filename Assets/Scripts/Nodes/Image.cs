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

    public SpriteRenderer SpriteRenderer => m_SpriteRenderer ??= GetComponent<SpriteRenderer>();

    public Vector2 Size
    {
        get => SpriteRenderer.sprite.bounds.size;
        set => SpriteRenderer.size = value;
    }

    public Color Color
    {
        get => SpriteRenderer.color;
        set => SpriteRenderer.color = value;
    }

    public ImageFlipType FlipType
    {
        set
        {
            switch (value)
            {
                case ImageFlipType.NONE:
                    SpriteRenderer.flipX = false;
                    SpriteRenderer.flipY = false;
                    break;
                case ImageFlipType.HORIZONTAL:
                    SpriteRenderer.flipX = false;
                    SpriteRenderer.flipY = true;
                    break;
                case ImageFlipType.VERTICAL:
                    SpriteRenderer.flipX = true;
                    SpriteRenderer.flipY = false;
                    break;
            }
        }
    }

    #endregion

    #region attributes

    SpriteRenderer m_SpriteRenderer;

    #endregion

    #region engine methods

    #endregion

    #region service methods

    void Create(Vector2 _Offset, string _Sprite)
    {
        base.Create(_Offset);

        if (_Sprite.Length != 0 && _Sprite != null)
            SpriteRenderer.sprite = Resources.Load<Sprite>($"Sprites/{_Sprite}");
    }

    #endregion
}

}