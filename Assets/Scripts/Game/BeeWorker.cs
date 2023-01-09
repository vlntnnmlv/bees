using UnityEngine;
using VN;

class BeeWorker : Bee
{
    #region constants

    const float DIRECTION_LERP_TIME = 0.3f;
    const float LIFE_TIME           = 20;

    #endregion

    #region attributes

    Vector2 m_PrevDirection = Vector2.zero;
    Vector2 m_NextDirection = Vector2.zero;

    float   m_DirectionChangedTime = 0;
    float   m_CreationTime = 0;

    #endregion

    #region service methods

    protected override void Create(Rect _Rect, float _Health, float _Speed, float _Damage)
    {
        base.Create(_Rect, _Health, _Speed, _Damage);

        m_CreationTime = Time.time;
    }

    protected override void Update()
    {
        if (!GotHoney && !IsFlowering && m_DropHoneyPotCoroutine == null && Time.time - m_CreationTime > LIFE_TIME)
        {
            Paused = true;
            Dead   = true;
        }

        UpdateDirectionSmooth();

        base.Update();
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