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

    new public static HealthBar Create(Node _Parent, Vector2 _Offset, string _ID)
    {
        HealthBar bar = Utility.LoadObject<HealthBar>("Prefabs/HealthBar", _ID, _Parent);

        bar.Create(_Offset);
        return bar;
    }

    public void UpdateBar(float _P)
    {
        _P = _P / BASE_HEALTH;
        Image[] parts = GetComponentsInChildren<Image>();
        foreach (Image part in parts)
            part.Size = new Vector2(part.Size.x * _P, part.Size.y);
    }

    public void UpdateBar(int _P)
    {
        UpdateBar((float) _P);
    }
}