using UnityEngine;
using System.Collections;
using VN;

public abstract class Bee : Character
{
    #region constants

    const float FLY_OFF_SPEED = 5;

    #endregion

    #region attributes

    [SerializeField] protected Node m_Pot;

    Animator     m_Animator;
    bool         m_IsFlowering;

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
        }
    }

    public bool GotHoney { get; set; }

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
        StartCoroutine(DropHoneyPotCoroutine(_Dest));
    }

    #endregion

    #region service methods

    protected void Create(Node _Parent, Vector2 _HiveOffset, Vector2 _FlyTo, float _Speed)
    {
        base.Create(_HiveOffset);
        Speed = _Speed;
        StartCoroutine(FlyToPoint(_FlyTo));
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
        StartCoroutine(FloweringCoroutine());
    }

    void SetHoneyPot()
    {
        StartCoroutine(SetHoneyPotCoroutine());
    }

    #endregion

    #region coroutines

    IEnumerator FlyToPoint(Vector2 _Dest)
    {
        Vector2 start = Offset;
        yield return Coroutines.Update(
            null,
            _Phase => Offset = Vector2.Lerp(start, _Dest, _Phase),
            null,
            Vector2.Distance(start, _Dest) / FLY_OFF_SPEED
        );
    }

    IEnumerator FloweringCoroutine()
    {
        IsFlowering = true;
        yield return new WaitForSeconds(0.5f);
        IsFlowering = false;
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
        HoneyPot tmpPot = HoneyPot.Create(null, m_Pot.Offset, "tmpPot", true);
        Vector2  start  = tmpPot.Offset;

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
                () => Destroy(tmpPot.gameObject),
                0.4f
            )
        );
    }

    #endregion
}