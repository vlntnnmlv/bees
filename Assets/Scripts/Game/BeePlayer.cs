using UnityEngine;
using VN;

public class BeePlayer : Bee
{
    #region properties

    Controller2D Controller
    {
        get
        {
            if (m_Controller == null)
                m_Controller = GetComponent<Controller2D>() ?? gameObject.AddComponent<Controller2D>();

            return m_Controller;
        }
    }

    public override bool IsHealthBarActive => base.IsHealthBarActive && IsDamaged;

    #endregion

    #region attributes

    Controller2D m_Controller;

    #endregion

    #region service methods

    protected override void Create(Rect _Rect, float _Health, float _Speed, float _Damage)
    {
        Controller.Init(this);

        base.Create(_Rect, _Health, _Speed, _Damage);
    }

    #endregion
}