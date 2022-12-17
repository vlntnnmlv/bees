using UnityEngine;
using System.Collections;
using VN;

public class BeePlayer : Bee
{
    #region creation
    
    public static BeePlayer Create(Node _Parent, Vector2 _Offset, string _ID, Vector2 _FlyTo)
    {
        BeePlayer beePlayer = Utility.LoadObject<BeePlayer>("Prefabs/BeePlayer", _ID, _Parent);

        beePlayer.Create(_Parent, _Offset, _FlyTo);
        return beePlayer;
    }

    #endregion

    #region properties

    Controller2D Controller
    {
        get
        {
            if (m_Controller == null)
            {
                m_Controller = GetComponent<Controller2D>() ?? gameObject.AddComponent<Controller2D>();
                m_Controller.Init(this);
            }

            return m_Controller;
        }
    }

    protected override float   Speed        => 5;
    public    override Vector2 FlyDirection => Controller.Direction;

    #endregion

    #region attributes

    Controller2D m_Controller;

    #endregion
}