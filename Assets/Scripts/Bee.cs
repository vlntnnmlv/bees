using UnityEngine;
using System.Collections;
using VN;

class Bee : Controllable
{
    #region attributes

    [SerializeField] Image    m_Stroke;
    [SerializeField] Node     m_Pot;

    Animator m_Animator;
    bool     m_IsFlowering;

    #endregion

    #region properties

    public bool            IsFlowering
    {
        get => m_IsFlowering;
        set
        {
            m_IsFlowering = value;
            m_Animator.SetBool("IsFlowering", m_IsFlowering);
        }
    }

    public bool            GotHoney   { get; set; }
    public Image           Stroke     => m_Stroke;

    Image[] Parts => GetComponentsInChildren<Image>(); 

    #endregion

    #region creation
    
    public static Bee Create(Node _Parent, Vector2 _Offset, string _ID, Vector2 _FlyTo)
    {
        Bee bee = Utility.LoadObject<Bee>("Prefabs/Bee", _ID, _Parent);

        bee.Create(_Parent, _Offset, _FlyTo);
        return bee;
    }

    #endregion
    
    #region service methods
    void Create(Node _Parent, Vector2 _HiveOffset, Vector2 _FlyTo)
    {
        base.Create(_HiveOffset);
        StartCoroutine(FlyToPoint(_FlyTo));
    }
    
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Controller = GetComponent<Controller2D>();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        Flower[] flowers = FindObjectsOfType<Flower>();
        foreach (Flower flower in flowers)
        {
            if (flower.CanBeUsed && !IsFlowering && !GotHoney && Vector2.Distance(Offset, flower.Offset) < 0.5f)
            {
                DoFlowering(flower);
            }
        }
    }

    protected override void OnPositionUpdate(Vector2 _Direction)
    {
        foreach (Image part in Parts)
            part.FlipType = _Direction.x < 0 
                ? ImageFlipType.VERTICAL
                : ImageFlipType.NONE;
    }

    protected override void OnChosen(bool _Chosen)
    {
        Stroke.gameObject.SetActive(_Chosen);
    }
    
    void DoFlowering(Flower _Flower)
    {
        IsFlowering = true;
        m_Controller.Pause(0.5f, () => 
                {
                    _Flower.Used = true;

                    IsFlowering = false;
                    SetHoneyPot();
                }
            );
    }

    void SetHoneyPot()
    {
        GotHoney = true;
        StartCoroutine(SetHoneyPotCoroutine());
    }

    #endregion

    #region public methods
    
    public void DropHoneyPot(Vector2 _Dest)
    {
        GotHoney = false;
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
            Vector2.Distance(start, _Dest) / Speed
        );
    }

    IEnumerator SetHoneyPotCoroutine()
    {
        yield return StartCoroutine(Coroutines.Update(
                () =>
                {
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
                null,
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