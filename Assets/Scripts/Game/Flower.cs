using UnityEngine;
using VN;

public class Flower : Character
{
    #region constants

    const float LIFE_TIME = 15;

    #endregion

    #region proprties

    public bool     Used { get; set; }
    public bool     CanBeUsed => m_Appeared && !Used;
    public override GroupType Group => GroupType.PEACEFULL;

    #endregion

    #region attributes

    float    m_CreationTime;

    #endregion

    #region engine methods

    protected override void Update()
    {
        base.Update();

        if (Time.time - m_CreationTime > LIFE_TIME)
        {
            Used = true;
            Health = 0;
        }
    }

    #endregion

    #region service methods

    protected override void Create(Rect _Rect, float _Health, float _Speed, float _Damage)
    {
        base.Create(_Rect, _Health, _Speed, _Damage);

        m_CreationTime = Time.time;
    }

    #endregion

    #region coroutines


    #endregion
}