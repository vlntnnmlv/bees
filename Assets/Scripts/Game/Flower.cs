using UnityEngine;
using VN;

public class Flower : Image
{
    const float LIFE_TIME = 15;

    public bool Used
    {
        get => m_Used;
        set
        {
            m_Used = value;
        }
    }

    public bool CanBeUsed => m_Appeared && !Used;

    [SerializeField] bool m_Appeared = false;
    Animator m_Animator;
    bool     m_Used;
    float    m_CreationTime;

    new public static Flower Create(string _ID, Node _Parent, Rect _Rect)
    {
        Flower flower = Utility.CreateObject<Flower>(_ID, _Parent);

        flower.Create(_Rect, "flower");

        return flower;
    }

    protected override void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    protected override void Create(Rect _Rect, string _SpriteName)
    {
        base.Create(_Rect, _SpriteName);

        // GetComponent<SpriteAnimated>().OnAnimationPlayed += () => m_Appeared = true;
        m_CreationTime = Time.time;
    }

    protected override void Update()
    {
        base.Update();

        if (Time.time - m_CreationTime > LIFE_TIME)
        {
            Used = true;
            Disappear();
        }
    }

    public void Disappear()
    {
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