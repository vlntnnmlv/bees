using UnityEngine;
using System.Collections;
using VN;

class BeeWorker : Bee
{
    #region creation
    
    public static BeeWorker Create(Node _Parent, Vector2 _Offset, string _ID, Vector2 _FlyTo)
    {
        BeeWorker bee = Utility.LoadObject<BeeWorker>("Prefabs/BeeWorker", _ID, _Parent);

        bee.Create(_Parent, _Offset, _FlyTo, 3.7f);
        return bee;
    }

    #endregion

    #region constants

    const float DIRECTION_LERP_TIME = 0.3f;

    #endregion

    #region attributes

    Vector2 m_PrevDirection = Vector2.zero;
    Vector2 m_NextDirection = Vector2.zero;

    float   m_DirectionChangedTime = 0;

    #endregion

    #region service methods

    protected override void OnUpdate()
    {
        UpdateDirectionSmooth();

        base.OnUpdate();
    }

    void UpdateDirectionSmooth()
    {
        if (GotHoney)
        {
            Direction = -Offset.normalized;
            return;
        }

        if (Time.time - m_DirectionChangedTime >= 1)
        {
            m_PrevDirection = Direction;
            m_NextDirection = Utility.RandomOffset.normalized;
            m_DirectionChangedTime = Time.time;
        }

        if (Time.time - m_DirectionChangedTime < DIRECTION_LERP_TIME)
            Direction = Vector2.Lerp(m_PrevDirection, m_NextDirection, (Time.time - m_DirectionChangedTime) / DIRECTION_LERP_TIME);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Offset, Offset + Direction);
    }

    #endregion
}