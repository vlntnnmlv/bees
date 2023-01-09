using UnityEngine;
using VN;

namespace VN
{

public enum EffectType
{
    Hit = 0,
    Disappear
}

public class Effect : Node
{

    #region creation

    public static Effect Create(string _ID, Node _Parent, Rect _Rect, EffectType _Type)
    {
        Effect effect = Utility.LoadObject<Effect>($"Prefabs/FX/{_Type.ToString()}Effect", _ID, _Parent);

        effect.Create(_Rect);
        return effect;
    }

    #endregion

    #region service methods

    new void Create(Rect _Rect)
    {
        base.Create(_Rect);

        Pivot = Vector2.one / 2;
        Offset += LocalRect.size / 2;

        m_ParticleSystem.textureSheetAnimation.AddSprite(m_Sprite.SpriteInternal);
    }

    #endregion

    #region attributes

    [SerializeField] ParticleSystem m_ParticleSystem;
    [SerializeField] VN.Sprite      m_Sprite;

    EffectType m_Type;

    #endregion


}

}