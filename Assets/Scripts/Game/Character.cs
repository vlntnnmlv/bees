using UnityEngine;
using System.Linq;
using VN;

public class Character : Node, IMovable
{
    [SerializeField] bool    m_HealthBarAlwaysActive;
    HealthBar m_HealthBar;
    float     m_Health;
    bool      m_Paused;

    public Vector2 Direction    { get; set; } = Vector2.zero;
    public float   Speed        { get; set; }

    public bool    Paused       { get; set; }

    public bool    Constrainted { get; set; } = true;
    public float   Health
    {
        get => m_Health;
        set
        {
            m_Health = value;
            if (value <= 0)
                m_Health = 0;

            if (m_HealthBar != null)
                m_HealthBar.UpdateBar(m_Health);
        }
    }

    protected Image[] Parts => GetComponentsInChildren<Image>();

    public bool    HealthBarAlwaysActive
    {
        get => m_HealthBarAlwaysActive;
        set => m_HealthBarAlwaysActive = value;
    }

    protected Vector2 Size
    {
        get
        {
            return new Vector2(
                Parts.Max(part => part.Size.x),
                Parts.Max(part => part.Size.y)
            );
        }
    }

    protected void Create(Vector2 _Offset, float _Health, float _Speed)
    {
        base.Create(_Offset);

        CreateHealthBar();

        Health = _Health;
        Speed  = _Speed;
    }

    void CreateHealthBar()
    {
        m_HealthBar = HealthBar.Create(this, new Vector2(0, Size.y * 0.55f), "HealthBar");
        m_HealthBar.Color = Color.red;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (!Paused)
        {
            Vector2 newOffset = Offset + Direction * Speed * Time.deltaTime;
            if (Constrainted)
            {
                if (newOffset.x > Utility.Width/2)
                    newOffset.x = -Utility.Width/2;
                if (newOffset.x < -Utility.Width/2)
                    newOffset.x = Utility.Width/2;

                if (newOffset.y > Utility.Height/2)
                    newOffset.y = -Utility.Height/2;
                if (newOffset.y < -Utility.Height/2)
                    newOffset.y = Utility.Height/2;
            }

            Offset = newOffset;
        }

        if (Direction.x > 0)
            OnTurn(false);
        if (Direction.x < 0)
            OnTurn(true);
    }

    void OnTurn(bool _Left)
    {
        foreach (Image part in Parts)
            part.FlipType = _Left ? ImageFlipType.VERTICAL : ImageFlipType.NONE;
    }
}