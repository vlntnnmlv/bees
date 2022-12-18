using System.Collections;
using UnityEngine;
using VN;

class HoneyPot : Image
{
    public static HoneyPot Create(Node _Parent, Vector2 _Offset, string _ID, bool _Instant = false)
    {
        HoneyPot honeyPot = Utility.LoadObject<HoneyPot>("Prefabs/HoneyPot", _ID,_Parent);
        honeyPot.Create(_Offset, _Instant);

        return honeyPot;
    }

    void Create(Vector2 _Offset, bool _Instant)
    {
        base.Create(_Offset);

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