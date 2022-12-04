using UnityEngine;
using System.Collections;
using VN;

class Bee : Controllable
{
    [SerializeField] Image m_Stroke;

    HoneyPot m_Pot;

    public bool            GotAPot    => m_Pot != null && !m_Pot.Dropped;
    public Image           Stroke     => m_Stroke;

    Image[] Parts => GetComponentsInChildren<Image>(); 

    public static Bee Create(Node _Parent, Vector2 _Offset, string _ID, Vector2 _FlyTo)
    {
        Bee bee = Utility.LoadObject<Bee>("Prefabs/Bee", _ID, _Parent);

        bee.Create(_Parent, _Offset, _FlyTo);
        return bee;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        HoneyPot[] pots = FindObjectsOfType<HoneyPot>();
        foreach (HoneyPot pot in pots)
        {
            if (pot.CanBePickedUp && Vector2.Distance(Offset, pot.Offset) < 0.5f)
                PickUpPot(pot);
        }
    }

    void Create(Node _Parent, Vector2 _HiveOffset, Vector2 _FlyTo)
    {
        base.Create(_HiveOffset);
        StartCoroutine(FlyToPoint(_FlyTo));
    }

    void PickUpPot(HoneyPot _Pot)
    {
        m_Pot = _Pot;
        
        m_Pot.PickedUp = true;
        StartCoroutine(PickUpPotCoroutine(_Pot));
    }

    public void DropPot(Vector2 _Dest)
    {
        if (!GotAPot)
            return;
        m_Pot.Dropped = true;
        m_Pot.PickedUp = false;
        m_Pot.SetParent(null);
        StartCoroutine(DropPotCoroutine(_Dest));
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

    IEnumerator FlyToPoint(Vector2 _Dest)
    {
        Vector2 start = Offset;
        yield return Coroutines.Update(
            null,
            _Phase => Offset = Vector2.Lerp(start, _Dest, _Phase),
            null,
            1.2f
        );
    }

    IEnumerator PickUpPotCoroutine(HoneyPot _Pot)
    {
        Vector2 start = _Pot.Offset;
        yield return Coroutines.Update(
            () => _Pot.SetParent(this, true),
            _Phase => { _Pot.Offset = Vector2.Lerp(start, Offset, _Phase); },
            null,
            0.2f
        );
    }

    IEnumerator DropPotCoroutine(Vector2 _Dest)
    {
        Vector2 start = m_Pot.Offset;
        yield return Coroutines.Update(
            null,
            _Phase => m_Pot.Offset = Vector2.Lerp(start, _Dest, _Phase),
            null,
            0.2f
        );

        yield return new WaitForSeconds(0.1f);

        yield return Coroutines.Update(
            null,
            _Phase => m_Pot.LocalScale = Vector2.Lerp(Vector2.one, Vector2.zero, _Phase),
            () => {
                Destroy(m_Pot.gameObject);
                m_Pot = null;
            },
            0.2f
        );
    }
}