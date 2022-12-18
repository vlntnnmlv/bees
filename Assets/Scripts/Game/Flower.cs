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
            m_Animator.SetTrigger("Used");
        }
    }

    public bool CanBeUsed => m_Appeared && !Used;

    [SerializeField] bool m_Appeared = false;
    Animator m_Animator;
    bool     m_Used;
    float    m_DisappearTime;

    new public static Flower Create(Node _Parent, Vector2 _Offset, string _ID)
    {
        Flower flower = Utility.LoadObject<Flower>("Prefabs/Flower", _ID, _Parent);

        flower.Create(_Offset);

        return flower;
    }

    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    new void Create(Vector2 _Offset)
    {
        base.Create(_Offset);

        GetComponent<SpriteAnimated>().OnAnimationPlayed += () => m_Appeared = true;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (Time.time > m_DisappearTime)
            m_Animator.SetTrigger("Disappear");
    }

    public void OnAppear()
    {
        m_Appeared = true;
    }

    public void OnDisappear()
    {
        Destroy(gameObject, 0.0f);
    }
}