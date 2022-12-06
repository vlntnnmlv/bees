using UnityEngine;
using VN;

public class Flower : Image
{
    public bool Used
    {
        get => m_Used;
        set
        {
            m_Used = value;
            m_Animator.SetBool("Used", m_Used);
        }
    }

    public bool CanBeUsed => m_Appeared && !Used;

    [SerializeField] bool m_Appeared = false;
    Animator m_Animator;
    bool     m_Used;
    public static Flower Create(Node _Parent, Vector2 _Offset, string _ID)
    {
        Flower flower = Utility.LoadObject<Flower>("Prefabs/Flower", _ID, _Parent);

        flower.Create(_Offset);

        return flower;
    }

    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    void Create(Vector2 _Offset)
    {
        base.Create(_Offset);

        GetComponent<SpriteAnimated>().OnAnimationPlayed += () => m_Appeared = true;
    }

    public void SetReady()
    {
        m_Appeared = true;
        Used = false;
    }
}