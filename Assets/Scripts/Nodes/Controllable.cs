using UnityEngine;

namespace VN
{

[RequireComponent(typeof(Controller2D))]
public class Controllable : Node
{

    Vector2 Direction => m_Controller.DefaultDirectionVector;
    protected float Speed => m_Speed;
    protected Controller2D           m_Controller;
    [SerializeField] float m_Speed;

    void Awake()
    {
        m_Controller = GetComponent<Controller2D>();
        m_Controller.OnChosen = OnChosen;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (m_Controller.Chosen)
        {
            Offset += Direction * Time.deltaTime * m_Speed;
            OnPositionUpdate(Direction);
        }
    }

    void OnMouseDown() 
    {
        m_Controller.Chosen = !m_Controller.Chosen;
    }

    protected virtual void OnPositionUpdate(Vector2 _Direction) {}
    protected virtual void OnChosen(bool _Chosen) {}
}

}