using UnityEngine;
using VN;

public class HealthBar : Node
{
    const float BASE_HEALTH = 100;
    [SerializeField] Image m_Bar;
    public Color Color
    {
        set => m_Bar.Color = value;
    }

    new public static HealthBar Create(string _ID, Node _Parent, Rect _Rect)
    {
        HealthBar bar = Utility.LoadObject<HealthBar>("Prefabs/HealthBar", _ID, _Parent);

        bar.Create(_Rect);
        return bar;
    }

    public void UpdateBar(float _P)
    {
        _P = _P / BASE_HEALTH;
        Image[] parts = GetComponentsInChildren<Image>();
        foreach (Image part in parts)
            part.LocalRect = new Rect(part.LocalRect.min, new Vector2(part.LocalRect.width * _P, part.LocalRect.height));
    }

    public void UpdateBar(int _P)
    {
        UpdateBar((float) _P);
    }
}