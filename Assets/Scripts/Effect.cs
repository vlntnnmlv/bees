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

    #region attributes

    [SerializeField] ParticleSystem m_ParticleSystem;
    EffectType m_Type;

    #endregion


}

}