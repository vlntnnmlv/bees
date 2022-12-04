using System.Collections;
using UnityEngine;
using VN;

class HoneyPot : Image
{
    public bool PickedUp { get; set; }
    public bool Dropped  { get; set; }

    [SerializeField] bool m_Appeared = false;

    public bool CanBePickedUp => m_Appeared && !PickedUp && !Dropped;

    public static HoneyPot Create(Node _Parent, Vector2 _Offset, string _ID)
    {
        HoneyPot honeyPot = Utility.LoadObject<HoneyPot>("Prefabs/HoneyPot", _ID,_Parent);
        honeyPot.Create(_Offset);

        return honeyPot;
    }

    void Create(Vector2 _Offset)
    {
        base.Create(_Offset);

        StartCoroutine(Appear());
    }

    IEnumerator Appear()
    {
        yield return Coroutines.Update(
            () => LocalScale = Vector2.zero,
            _Phase => LocalScale = Vector2.Lerp(Vector2.zero, Vector2.one, _Phase),
            () => m_Appeared = true,
            0.5f
        );
    }


}