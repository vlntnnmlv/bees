using UnityEngine;
using System.Collections;


class Bee : VN.Object
{
    VN.Controller2D m_Controller;

    [SerializeField] float m_Speed;
    [SerializeField] GameObject m_Stroke;

    HoneyPot m_Pot;

    public bool GotAPot => m_Pot != null && !m_Pot.Dropped;
    public VN.Controller2D Controller => m_Controller;
    public GameObject      Stroke     => m_Stroke;

    public static Bee Create(string _ID, VN.Object _Parent, Vector2 _HiveOffset, Vector2 _Offset)
    {
        Bee bee = Instantiate(Resources.Load<Bee>("Prefabs/Bee"));
        bee.name = _ID;
        bee.Create(_Parent, _HiveOffset, _Offset);
        return bee;
    }

    void Start()
    {
        m_Controller = GetComponent<VN.Controller2D>();
        m_Stroke.gameObject.SetActive(Controller.Chosen);
    }

    void OnMouseDown() 
    {
        Bee[] bees = FindObjectsOfType<Bee>();
        foreach (Bee bee in bees)
        {
            bee.Controller.Chosen = bee.name == name;
        }
    }

    void Update()
    {
        Stroke.gameObject.SetActive(m_Controller.Chosen);

        if (m_Controller.Chosen)
        {
            Offset += m_Controller.DefaultDirectionVector * Time.deltaTime * m_Speed;
            foreach (SpriteRenderer r in SpriteRenderers)
                r.flipX = m_Controller.DefaultDirectionVector.x < 0;
        }

        HoneyPot[] pots = FindObjectsOfType<HoneyPot>();
        foreach (HoneyPot pot in pots)
        {
            if (pot.CanBePickedUp && Vector2.Distance(Offset, pot.Offset) < 0.5f)
                PickUpPot(pot);
        }
    }

    void Create(VN.Object _Parent, Vector2 _HiveOffset, Vector2 _Offset)
    {
        base.Create(_HiveOffset, _Parent);
        StartCoroutine(FlyToPoint(_Offset));
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
            () => _Pot.SetParent(this),
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