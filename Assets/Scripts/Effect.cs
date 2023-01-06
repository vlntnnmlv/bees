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

    public static Effect Create(Node _Parent, Vector2 _Offset, string _ID, EffectType _Type)
    {
        Effect effect = Utility.LoadObject<Effect>($"Prefabs/FX/{_Type.ToString()}Effect", _ID, _Parent);

        effect.Create(_Offset);
        return effect;
    }

    #endregion

    #region attributes

    [SerializeField] ParticleSystem m_ParticleSystem;
    EffectType m_Type;

    #endregion


}

}