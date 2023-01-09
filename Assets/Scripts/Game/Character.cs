using UnityEngine;
using System.Collections;
using VN;
using TMPro;

public class Character : Node, IMovable
{
    #region creation

    public static T Create<T>(string _ID, Node _Parent, Rect _Rect, CharacterConfig _Config) where T : Character
    {
        T character = Utility.LoadObject<T>($"Prefabs/{_Config.PrefabName}", _ID, _Parent);

        character.Create(_Rect, _Config.Health, _Config.Speed, _Config.Damage);
        return character;
    }

    #endregion

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
    protected bool           m_Appeared;
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

    GameManager GameManager => FindObjectOfType<GameManager>();

    #endregion

    #region engine methods

    void LateUpdate()
    {
        if (Dead)
        {
            Effect.Create("disappear_fx", null, WorldRect, EffectType.Disappear);
            Destroy(gameObject);
        }
    }

    #endregion

    #region service methods

    protected virtual void Create(Rect _Rect, float _Health, float _Speed, float _Damage)
    {
        base.Create(_Rect);

        Pivot = Vector2.one / 2;

        CreateHealthBar();

        Health = _Health;
        Speed  = _Speed;
        Damage = _Damage;

        Appear();
    }

    protected override void Update()
    {
        base.Update();

        DoAttack();
        DoMovement();

        m_HealthBar.gameObject.SetActive(IsHealthBarActive);
    }

    protected void Appear()
    {
        StartCoroutine(AppearCoroutine());
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
        if (!Paused && m_Appeared)
        {
            Vector2 newOffset = Offset + Direction * Speed * Time.deltaTime;
            if (Constrainted)
            {
                if (newOffset.x > Utility.ScreenWidth/2)
                    newOffset.x = -Utility.ScreenWidth/2;
                if (newOffset.x < -Utility.ScreenWidth/2)
                    newOffset.x = Utility.ScreenWidth/2;

                if (newOffset.y > Utility.ScreenHeight/2)
                    newOffset.y = -Utility.ScreenHeight/2;
                if (newOffset.y < -Utility.ScreenHeight/2)
                    newOffset.y = Utility.ScreenHeight/2;
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
        LocalScale = _Left ? new Vector2(-1, 1) : Vector2.one;
    }

    void CreateHealthBar()
    {
        m_HealthBar = HealthBar.Create("HealthBar", this, Rect.zero);

        m_HealthBar.Pivot = Vector2.one / 2;
        m_HealthBar.Anchors = new Anchors(0, 1, 1.1f, 1.2f);

        m_HealthBar.Color = Group == GroupType.HOSTILE ? Color.red : Color.green;
    }

    #endregion

    #region coroutines

    IEnumerator AttackCoroutine(Character _Enemy)
    {
        Rect enemyRect = _Enemy.WorldRect;

        // TODO: Refactor this with Text node  (TextMesh)
        GameManager.StartCoroutine(
            ShowDamageCoroutine(
                    _Enemy.Offset,
                    Vector2.one * Mathf.Max(enemyRect.width, enemyRect.height),
                    Damage,
                    _Enemy.Group == GroupType.HOSTILE ? Color.green : Color.red
                    )
                );

        _Enemy.Health -= Damage;
        SoundMaker.PlaySound("hit", _Enemy.Offset);

        // create hit effect if enemy attack was not lethal
        if (_Enemy.Health != 0)
        {
            Effect.Create("hit_effect", _Enemy, _Enemy.LocalRect, EffectType.Hit);
        }

        // wait for attack delay
        yield return new WaitForSeconds(AttackDelay);

        m_AttackCoroutine = null;
    }

    IEnumerator ShowDamageCoroutine(Vector2 _Pos, Vector2 _Size, float _Damage, Color _Color)
    {
        TextMeshProUGUI txt = Instantiate(Resources.Load<TextMeshProUGUI>("Prefabs/UI/Text"));
        txt.transform.position = _Pos;
        txt.text = _Damage.ToString();
        txt.transform.localScale = _Size;
        Color startColor = _Color;
        txt.color = startColor;

        Vector2 posStart = txt.transform.position;
        yield return Coroutines.Update(
            null,
            _Phase => {
                txt.transform.position = Vector2.Lerp(posStart, posStart + Vector2.one * _Size / 2, _Phase);
                txt.color = Color.Lerp(startColor, Color.clear, _Phase);
            },
            () => Destroy(txt.gameObject),
            1f
        );
    }

    IEnumerator AppearCoroutine()
    {
        yield return AppearCoroutineInternal();
        m_Appeared = true;
    }

    protected IEnumerator AppearCoroutineInternal()
    {
        yield return Coroutines.Update(
            null,
            _Phase => LocalScale = Vector2.Lerp(Vector2.zero, Vector2.one, _Phase),
            null,
            0.25f
        );
    }

    #endregion
}