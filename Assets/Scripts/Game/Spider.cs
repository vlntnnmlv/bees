using UnityEngine;
using VN;

public class Spider : Character
{
    #region attributes

    Vector2 m_Destination;

    #endregion

    #region properties

    public override GroupType Group => GroupType.HOSTILE;

    #endregion

    #region service methods

    protected override void Create(Rect _Rect, float _Health, float _Speed, float _Damage)
    {
        base.Create(_Rect, _Health, _Speed, _Damage);

        m_Destination = Utility.RandomCenterOffset;
    }

    protected override void Update()
    {
        Direction = (m_Destination - Offset).normalized;

        base.Update();
    }

    #endregion

}