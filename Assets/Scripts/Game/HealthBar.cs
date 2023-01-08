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

    public void UpdateBar(float _Health)
    {
        _Health = _Health / BASE_HEALTH;
        Image[] parts = GetComponentsInChildren<Image>();
        foreach (Image part in parts)
            part.LocalScale = new Vector2(_Health, part.LocalScale.y);
    }

    public void UpdateBar(int _P)
    {
        UpdateBar((float) _P);
    }
}