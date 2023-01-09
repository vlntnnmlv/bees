using UnityEngine;
using System.Collections;
using VN;

public abstract class Bee : Character
{
    #region attributes

    [SerializeField] protected Node m_Pot;

    protected Animator  m_Animator;
    protected Coroutine m_DropHoneyPotCoroutine = null;
    protected Coroutine m_SetHoneyPotCoroutine  = null;

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
                SoundMaker.PlaySound("flowering", Offset);
        }
    }

    public          bool      GotHoney { get; set; }
    public override GroupType Group => GroupType.FRIENDLY;

    #endregion
    
    #region engine methods

    protected override void Start()
    {
        base.Start();

        m_Animator = GetComponent<Animator>();
    }

    #endregion

    #region public methods
    
    public void DropHoneyPot(Vector2 _Dest)
    {
        m_DropHoneyPotCoroutine = StartCoroutine(DropHoneyPotCoroutine(_Dest));
    }

    #endregion

    #region service methods

    protected override void Update()
    {
        base.Update();

        Flower[] flowers = FindObjectsOfType<Flower>();
        foreach (Flower flower in flowers)
        {
            if (flower.CanBeUsed && !IsFlowering && !GotHoney && flower.WorldRect.Intersects(WorldRect))
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
        m_SetHoneyPotCoroutine = StartCoroutine(SetHoneyPotCoroutine());
    }

    #endregion

    #region coroutines

    IEnumerator FloweringCoroutine(Flower _Flower)
    {
        IsFlowering = true;
        yield return new WaitForSeconds(1);
        IsFlowering = false;
        _Flower.Health = 0;
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
                () =>
                {
                    m_SetHoneyPotCoroutine = null;
                    SoundMaker.PlaySound("pop", WorldOffset);
                },
                0.1f
            )
        );
    }

    IEnumerator DropHoneyPotCoroutine(Vector2 _Dest)
    {
        GotHoney = false;
        yield return new WaitWhile(() => m_SetHoneyPotCoroutine != null);

        m_Pot.gameObject.SetActive(false);
        HoneyPot tmpPot = HoneyPot.Create("tmpPot", null, m_Pot.WorldRect, true);
        tmpPot.Rotation = m_Pot.Rotation;
        tmpPot.WorldOffset = m_Pot.WorldOffset;

        Vector2  start  = tmpPot.WorldOffset;

        yield return StartCoroutine(Coroutines.Update(
                () => SoundMaker.PlaySound("pop", WorldOffset),
                _Phase => tmpPot.WorldOffset = Vector2.Lerp(start, _Dest, _Phase),
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