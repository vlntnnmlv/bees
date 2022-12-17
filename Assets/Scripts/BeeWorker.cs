using UnityEngine;
using System.Collections;
using VN;

class BeeWorker : Bee
{
    #region creation
    
    public static BeeWorker Create(Node _Parent, Vector2 _Offset, string _ID, Vector2 _FlyTo)
    {
        BeeWorker bee = Utility.LoadObject<BeeWorker>("Prefabs/BeeWorker", _ID, _Parent);

        bee.Create(_Parent, _Offset, _FlyTo);
        return bee;
    }

    #endregion

    #region properties

    protected override float   Speed        => 3.5f;
    public    override Vector2 FlyDirection => GetSmoothDirection();

    #endregion

    #region attributes

    Vector2 m_PrevDirection = Vector2.zero;
    Vector2 m_NextDirection = Vector2.zero;
    Vector2 m_Direction     = Vector2.zero;

    float   m_DirectionChangedTime = 0;
    float   m_DirectionLerpTime = 0.3f;

    #endregion

    #region service methods

    Vector2 GetSmoothDirection()
    {
        if (GotHoney)
        {
            m_Direction = -Offset.normalized;
            return m_Direction;
        }

        if (Time.time - m_DirectionChangedTime >= 1)
        {
            m_PrevDirection = m_Direction;
            m_NextDirection = Utility.RandomOffset.normalized;
            m_DirectionChangedTime = Time.time;
        }

        if (Time.time - m_DirectionChangedTime < m_DirectionLerpTime)
            m_Direction = Vector2.Lerp(m_PrevDirection, m_NextDirection, (Time.time - m_DirectionChangedTime) / m_DirectionLerpTime);

        return m_Direction;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Offset, Offset + m_Direction);
    }

    #endregion
}