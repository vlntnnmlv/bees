using UnityEngine;

namespace VN
{

[RequireComponent(typeof(Controller2D))]
public class Controllable : Node
{

    protected Vector2 Direction     => m_Controller.DefaultDirectionVector;
    protected float Speed => m_Speed;
    protected Controller2D m_Controller;
    [SerializeField] float m_Speed;
    Vector2? m_Clicked = null;

    void Awake()
    {
        m_Controller = GetComponent<Controller2D>();
        m_Controller.OnChosen = OnChosen;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (Input.GetMouseButtonDown(0))
            m_Clicked = Utility.MousePositionWorld;

        if (m_Controller.Chosen)
        {
            if (m_Clicked.HasValue)
            {
                if ((m_Clicked.Value - Offset).sqrMagnitude > 0.01f)
                    UpdatePosition((m_Clicked.Value - Offset).normalized * Time.deltaTime * m_Speed);
                else
                    m_Clicked = null;
                return;
            }

            UpdatePosition(Direction * Time.deltaTime * m_Speed);
        }
        else
            m_Clicked = null;
    }

    void UpdatePosition(Vector2 _OffsetDiff)
    {
        Offset += _OffsetDiff;
        OnPositionUpdate(Direction);
    }

    void OnMouseDown() 
    {
        m_Controller.Chosen = !m_Controller.Chosen;
    }

    protected virtual void OnPositionUpdate(Vector2 _Direction) {}
    protected virtual void OnChosen(bool _Chosen) {}
}

}