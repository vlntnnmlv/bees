using UnityEngine;
using System.Collections;
using VN;

public class Spider : Character
{
    #region creation

    public static Spider Create(Node _Parent, Vector2 _Offset, string _ID, float _Health = 25, float _Speed = 1, float _Damage = 4)
    {
        Spider spider = Utility.LoadObject<Spider>("Prefabs/Spider", _ID, _Parent);

        spider.Create(_Offset, _Health, _Speed, _Damage);
        return spider;
    }

    #endregion

    #region attributes

    Vector2 m_Destination;

    #endregion

    #region properties

    public override GroupType Group => GroupType.HOSTILE;

    #endregion

    #region service methods

    new void Create(Vector2 _Offset, float _Health, float _Speed, float _Damage)
    {
        base.Create(_Offset, _Health, _Speed, _Damage);

        m_Destination = Utility.RandomCenterOffset;
    }

    protected override void OnUpdate()
    {
        Direction = (m_Destination - Offset).normalized;

        base.OnUpdate();

        foreach (SpriteAnimated sprite in GetComponentsInChildren<SpriteAnimated>())
            sprite.Playing = (Offset - m_Destination).sqrMagnitude > 0.001f;
    }

    #endregion

}