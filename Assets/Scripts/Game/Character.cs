using UnityEngine;
using System.Linq;
using System.Collections;
using VN;

public class Character : Node, IMovable
{
    #region inner types

    public enum GroupType
    {
        FRIENDLY,
        PEACEFULL,
        HOSTILE
    }

    #endregion

    #region attributes

    [SerializeField] bool    m_IsHealthBarActive;
    HealthBar m_HealthBar;
    float     m_Health;
    bool      m_Paused;
    Coroutine m_AttackCoroutine;


    #endregion

    #region properties

    public bool    Paused         { get; set; }
    public bool    Constrainted   { get; set; } = true;
    public Vector2 Direction      { get; set; } = Vector2.zero;
    public float   Speed          { get; set; }
    public float   Damage         { get; set; }
    public float   AttackDistance { get; set; } = 0.5f;
    public float   AttackDelay    { get; set; } = 0.5f;

    public virtual bool      IsHealthBarActive => m_IsHealthBarActive;
    public virtual GroupType Group             => GroupType.PEACEFULL;

    public float   Health
    {
        get => m_Health;
        set
        {
            if (value < m_Health)
                IsDamaged = true;

            m_Health = value;
            if (value <= 0)
            {
                m_Health = 0;
                Destroy(gameObject);
            }

            if (IsHealthBarActive && m_HealthBar != null)
                m_HealthBar.UpdateBar(m_Health);
        }
    }

    protected bool IsDamaged { get; set; }

    protected Image[] Parts => GetComponentsInChildren<Image>();

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

    #endregion

    #region service methods

    protected void Create(Vector2 _Offset, float _Health, float _Speed, float _Damage)
    {
        base.Create(_Offset);

        CreateHealthBar();

        Health = _Health;
        Speed  = _Speed;
        Damage = _Damage;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        Character[] characters = FindObjectsOfType<Character>();
        foreach (Character character in characters)
        {
            if (ShouldAttack(character) && Vector2.Distance(Offset, character.Offset) < AttackDistance && m_AttackCoroutine == null)
            {
                Debug.Log($"[Character] {this.name} attacks {character.name} with {Damage} damage.");
                m_AttackCoroutine = StartCoroutine(AttackCoroutine(character));
            }
        }

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

        m_HealthBar.gameObject.SetActive(IsHealthBarActive);
    }

    void CreateHealthBar()
    {
        m_HealthBar = HealthBar.Create(this, new Vector2(0, Size.y * 0.55f), "HealthBar");
        m_HealthBar.Color = Color.red;
    }

    void OnTurn(bool _Left)
    {
        foreach (Image part in Parts)
            part.FlipType = _Left ? ImageFlipType.VERTICAL : ImageFlipType.NONE;
    }

    bool ShouldAttack(Character _Character)
    {
        GroupType group = _Character.Group;

        switch (Group)
        {
            case GroupType.FRIENDLY:
                return group == GroupType.HOSTILE;
            case GroupType.PEACEFULL:
                return false;
            case GroupType.HOSTILE:
                return group == GroupType.FRIENDLY || group == GroupType.PEACEFULL;
            default:
                return false;
        }
    }

    #endregion

    #region coroutines

    IEnumerator AttackCoroutine(Character _Enemy)
    {
        _Enemy.Health -= Damage;

        yield return new WaitForSeconds(AttackDelay);

        m_AttackCoroutine = null;
    }

    #endregion
}