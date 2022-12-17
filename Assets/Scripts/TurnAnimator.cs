using UnityEngine;
using System;

namespace VN
{

public class TurnAnimator : MonoBehaviour
{
    public Action OnTurnRight { get; set; }
    public Action OnTurnLeft  { get; set; }
    public Action OnTurnUp    { get; set; }
    public Action OnTurnDown  { get; set; }

    public Bee Bee
    {
        get
        {
            if (m_Bee == null)
                m_Bee = GetComponent<Bee>();
            
            return m_Bee;
        }
    }

    Bee m_Bee;

    public void Update()
    {
        if (Bee.FlyDirection.x > 0)
            OnTurnRight?.Invoke();
        if (Bee.FlyDirection.x < 0)
            OnTurnLeft?.Invoke();
        if (Bee.FlyDirection.y > 0)
            OnTurnUp?.Invoke();
        if (Bee.FlyDirection.y < 0)
            OnTurnDown?.Invoke();
    }
}

}