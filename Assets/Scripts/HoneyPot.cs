using UnityEngine;
using System;
using System.Collections;

class HoneyPot : VN.Object
{
    public bool PickedUp { get; set; }
    public bool Dropped  { get; set; }
    
    [SerializeField] bool m_Appeared = false;

    public bool CanBePickedUp => m_Appeared && !PickedUp && !Dropped;

    public static HoneyPot Create(string _ID, VN.Object _Parent, Vector2 _Offset)
    {
        HoneyPot honeyPot = ResourceManager.Load<HoneyPot>("Prefabs/HoneyPot", _ID);
        honeyPot.Create(_Parent, _Offset);
        return honeyPot;
    }

    void Create(VN.Object _Parent, Vector2 _Offset)
    {
        base.Create(_Offset, _Parent);

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