using UnityEngine;
using System;

namespace VN
{

public class Controller2D : MonoBehaviour
{
    #region attributes

    IMovable m_Movable;

    #endregion

    #region public methods

    public void Init(IMovable _Movable)
    {
        m_Movable = _Movable;
    }

    #endregion

    #region engine methods

    void Update()
    {
        CalculateDirection();
    }

    #endregion

    #region service methods

    void CalculateDirection()
    {
        if (m_Movable == null)
            return;

        Vector2 newDirection = Vector2.zero;

        {
            // click controls and keyboard controls
            if (Input.GetMouseButton(0))
            {
                newDirection = (Utility.MousePositionWorld - m_Movable.Offset);
            }
            else
            {
                if (Input.GetKey("w"))
                    newDirection += Vector2.up;
                if (Input.GetKey("a"))
                    newDirection += Vector2.left;
                if (Input.GetKey("s"))
                    newDirection += Vector2.down;
                if (Input.GetKey("d"))
                    newDirection += Vector2.right;
            }
        }

        m_Movable.Direction = newDirection.normalized;
    }

    #endregion
}

}