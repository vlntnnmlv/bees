using UnityEngine;
using System.Collections;
using VN;

public abstract class Bee : Node, IMovable
{
    #region attributes

    [SerializeField] protected Node m_Pot;

    Animator     m_Animator;
    bool         m_IsFlowering;

    #endregion

    #region properties

    protected virtual  float   FlyOffSpeed => 5f; 
    protected abstract float   Speed        { get; }
    public    abstract Vector2 FlyDirection { get; }

    public bool IsFlowering
    {
        get => m_IsFlowering;
        set
        {
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
        TurnAnimator turnAnimator = GetComponent<TurnAnimator>();
        turnAnimator.OnTurnLeft  = () => OnTurn(true);
        turnAnimator.OnTurnRight = () => OnTurn(false);
    }

    #endregion

    #region service methods

    protected void Create(Node _Parent, Vector2 _HiveOffset, Vector2 _FlyTo)
    {
        base.Create(_HiveOffset);
        StartCoroutine(FlyToPoint(_FlyTo));
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (!IsFlowering)
        {
            Vector2 newOffset = Offset + FlyDirection * Speed * Time.deltaTime;
            if (newOffset.x > Utility.Width/2)
                newOffset.x = -Utility.Width/2;
            if (newOffset.x < -Utility.Width/2)
                newOffset.x = Utility.Width/2;

            if (newOffset.y > Utility.Height/2)
                newOffset.y = -Utility.Height/2;
            if (newOffset.y < -Utility.Height/2)
                newOffset.y = Utility.Height/2;

            Offset = newOffset;
        }

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

    void OnTurn(bool _Left)
    {
        foreach (Image part in GetComponentsInChildren<Image>())
            part.FlipType = _Left ? ImageFlipType.VERTICAL : ImageFlipType.NONE;
    }

    #endregion

    #region public methods
    
    public void DropHoneyPot(Vector2 _Dest)
    {
        StartCoroutine(DropHoneyPotCoroutine(_Dest));
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
            Vector2.Distance(start, _Dest) / FlyOffSpeed
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
                0.6f
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