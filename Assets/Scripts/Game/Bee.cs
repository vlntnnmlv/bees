using UnityEngine;
using System.Collections;
using VN;

public abstract class Bee : Character
{
    #region attributes

    [SerializeField] protected Node m_Pot;

    protected Animator  m_Animator;
    protected Coroutine m_DropHoneyPotCoroutine = null;
    bool                m_IsFlowering;
    Vector2             m_FlyTo;

    #endregion

    #region properties

    public bool IsFlowering
    {
        get => m_IsFlowering;
        set
        {
            Paused = value;
            m_IsFlowering = value;
            m_Animator.SetBool("IsFlowering", m_IsFlowering);
            if (m_IsFlowering)
                SoundMaker.PlaySound("flowering", WorldOffset);
        }
    }

    public          bool      GotHoney { get; set; }
    public override GroupType Group => GroupType.FRIENDLY;

    #endregion
    
    #region engine methods

    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    #endregion

    #region public methods
    
    public void DropHoneyPot(Vector2 _Dest)
    {
        m_DropHoneyPotCoroutine = StartCoroutine(DropHoneyPotCoroutine(_Dest));
    }

    public void OnDied()
    {
        Destroy(gameObject, 0.0f);
    }

    #endregion

    #region service methods

    protected void Create(Node _Parent, Vector2 _HiveOffset, Vector2 _FlyTo, float _Health, float _Speed, float _Damage)
    {
        base.Create(_HiveOffset, _Health, _Speed, _Damage);

        m_FlyTo = _FlyTo;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        Flower[] flowers = FindObjectsOfType<Flower>();
        foreach (Flower flower in flowers)
        {
            if (flower.CanBeUsed && !IsFlowering && !GotHoney && Vector2.Distance(Offset, flower.Offset) < 0.5f)
                DoFlowering(flower);
        }
    }

    void DoFlowering(Flower _Flower)
    {
        _Flower.Used = true;
        StartCoroutine(FloweringCoroutine(_Flower));
    }

    void SetHoneyPot()
    {
        StartCoroutine(SetHoneyPotCoroutine());
    }

    #endregion

    #region coroutines

    protected override IEnumerator DefaultAppearCoroutine()
    {
        yield return StartCoroutine(FlyToPoint());
    }

    IEnumerator FlyToPoint()
    {
        Vector2 start = Offset;
        yield return Coroutines.Update(
            null,
            _ => Direction = (m_FlyTo - start).normalized,
            null,
            Vector2.Distance(start, m_FlyTo) / Speed
        );
    }

    IEnumerator FloweringCoroutine(Flower _Flower)
    {
        IsFlowering = true;
        yield return new WaitForSeconds(1);
        IsFlowering = false;
        _Flower.Disappear();
        SetHoneyPot();
    }

    IEnumerator SetHoneyPotCoroutine()
    {
        yield return StartCoroutine(Coroutines.Update(
                () =>
                {
                    GotHoney = true;
                    m_Pot.LocalScale = Vector2.zero;
                    m_Pot.gameObject.SetActive(true);
                },
                _Phase => m_Pot.LocalScale = _Phase * Vector2.one,
                null,
                0.35f
            )
        );
    }

    IEnumerator DropHoneyPotCoroutine(Vector2 _Dest)
    {
        m_Pot.gameObject.SetActive(false);
        HoneyPot tmpPot = HoneyPot.Create(null, transform.TransformPoint(m_Pot.Offset), "tmpPot", true);
        tmpPot.transform.rotation = m_Pot.transform.rotation;
        Vector2  start  = tmpPot.Offset;

        FindObjectOfType<GameManager>().IncreseScore();

        yield return StartCoroutine(Coroutines.Update(
                () => GotHoney = false,
                _Phase => tmpPot.Offset = Vector2.Lerp(start, _Dest, _Phase),
                null,
                0.5f
            )
        );

        yield return StartCoroutine(Coroutines.Update(
                null,
                _Phase => tmpPot.LocalScale = Vector2.one * (1 - _Phase),
                () =>
                {
                    m_DropHoneyPotCoroutine = null;
                    Destroy(tmpPot.gameObject);
                },
                0.4f
            )
        );
    }

    #endregion
}