using UnityEngine;
using System.Linq;
using System.Collections;
using VN;
using TMPro;

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
    HealthBar                m_HealthBar;
    float                    m_Health;
    bool                     m_Paused;
    Coroutine                m_AttackCoroutine;

    #endregion

    #region properties

    public bool    Paused         { get; set; }
    public bool    Dead           { get; set; }
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
                Dead = true;
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

    #region engine methods

    void LateUpdate()
    {
        if (Dead)
        {
            InitDisappearEffect(Offset);
            Destroy(gameObject);
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

        DoAttack();
        DoMovement();

        m_HealthBar.gameObject.SetActive(IsHealthBarActive);
    }


    void DoAttack()
    {
        Character[] characters = FindObjectsOfType<Character>();
        foreach (Character character in characters)
        {
            if (ShouldAttack(character))
            {
                Debug.Log($"[Character] {this.name} attacks {character.name} with {Damage} damage.");

                Attack(character);
            }
        }
    }

    bool ShouldAttack(Character _Character)
    {
        GroupType group = _Character.Group;

        if (Vector2.Distance(Offset, _Character.Offset) > AttackDistance || m_AttackCoroutine != null)
            return false;

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

    void Attack(Character _Character)
    {
        m_AttackCoroutine = StartCoroutine(AttackCoroutine(_Character));
    }

    void DoMovement()
    {
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

    GameObject InitHitEffect(Character _Character)
    {
        GameObject fx = Utility.Load<GameObject>("Prefabs/FX/HitEffect");
        fx.transform.SetParent(_Character.transform);
        fx.transform.localPosition = Vector2.zero;
        return fx;
    }

    GameObject InitDisappearEffect(Vector2 _Pos)
    {
        GameObject fx = Utility.Load<GameObject>("Prefabs/FX/DisappearEffect");
        fx.transform.position = _Pos;
        return fx;
    }

    #endregion

    #region coroutines

    IEnumerator AttackCoroutine(Character _Enemy)
    {
        Vector2 enemyOffset = _Enemy.Offset;

        _Enemy.Health -= Damage;
        // StartCoroutine(ShowDamageCoroutine(_Enemy, Damage));

        if (_Enemy.Health == 0)
            FindObjectOfType<GameManager>().IncresePoints();

        // create effect depending on health
        GameObject fx = _Enemy.Health == 0 ? InitDisappearEffect(enemyOffset) : InitHitEffect(_Enemy);

        // wait for attack delay
        yield return new WaitForSeconds(AttackDelay);

        m_AttackCoroutine = null;
    }

    IEnumerator ShowDamageCoroutine(Character _Character, float _Damage)
    {
        TextMeshProUGUI txt = Instantiate(Resources.Load<TextMeshProUGUI>("Prefabs/UI/Text"));
        txt.transform.position = _Character.transform.TransformPoint(_Character.Offset);
        txt.text = _Damage.ToString();

        Vector2 posStart = txt.transform.position;
        yield return Coroutines.Update(
            null,
            _Phase => {
                txt.transform.position = Vector2.Lerp(posStart, posStart + Vector2.one * Size / 2, _Phase);
                txt.color = Color.Lerp(Color.black, Color.clear, _Phase);
            },
            () => Destroy(txt.gameObject),
            0.5f
        );
    }

    #endregion
}