using System.Collections;
using UnityEngine;
using VN;

class HoneyPot : Image
{
    public static HoneyPot Create( string _ID, Node _Parent, Rect _Rect, bool _Instant = false)
    {
        HoneyPot honeyPot = Utility.CreateObject<HoneyPot>(_ID,_Parent);
        honeyPot.Create(_Rect, _Instant);

        return honeyPot;
    }

    void Create(Rect _Rect, bool _Instant)
    {
        base.Create(_Rect, "honey_pot");

        Pivot = Vector2.one / 2;

        if (!_Instant)
            StartCoroutine(Appear());
    }

    IEnumerator Appear()
    {
        yield return Coroutines.Update(
            () => LocalScale = Vector2.zero,
            _Phase => LocalScale = Vector2.Lerp(Vector2.zero, Vector2.one, _Phase),
            null,
            0.5f
        );
    }

    public IEnumerator Disappear()
    {
        yield return Coroutines.Update(
            null,
            _Phase => LocalScale = Vector2.Lerp(Vector2.one, Vector2.zero, _Phase),
            () => {
                Destroy(gameObject);
            },
            0.2f
        );
    }

}