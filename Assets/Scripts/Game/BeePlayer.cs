using UnityEngine;
using System.Collections;
using VN;

public class BeePlayer : Bee
{
    #region creation
    
    public static BeePlayer Create(Node _Parent, Vector2 _Offset, string _ID, Vector2 _FlyTo, float _Health = 100,float _Speed = 5)
    {
        BeePlayer beePlayer = Utility.LoadObject<BeePlayer>("Prefabs/BeePlayer", _ID, _Parent);

        beePlayer.Create(_Parent, _Offset, _FlyTo, _Health, _Speed);
        return beePlayer;
    }

    #endregion

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

    #endregion

    #region attributes

    Controller2D m_Controller;

    #endregion

    #region service methods

    new void Create(Node _Parent, Vector2 _Offset, Vector2 _FlyTo, float _Health, float _Speed)
    {
        Controller.Init(this);

        base.Create(_Parent, _Offset, _FlyTo, _Health, _Speed);
    }

    #endregion
}