using UnityEngine;
using VN;

public abstract class Character : Node, IMovable
{
    public Vector2 Direction { get; set; } = Vector2.zero;
    public float   Speed     { get; set; }
    public bool    Paused    { get; set; }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (!Paused)
        {
            Vector2 newOffset = Offset + Direction * Speed * Time.deltaTime;
            if (newOffset.x > Utility.Width/2)
                newOffset.x = -Utility.Width/2;
            if (newOffset.x < -Utility.Width/2)
                newOffset.x = Utility.Width/2;

            if (newOffset.y > Utility.Height/2)
                newOffset.y = -Utility.Height/2;
            if (newOffset.y < -Utility.Height/2)
                newOffset.y = Utility.Height/2;

            Offset = newOffset;
        }

        if (Direction.x > 0)
            OnTurn(false);
        if (Direction.x < 0)
            OnTurn(true);
    }

    void OnTurn(bool _Left)
    {
        foreach (Image part in GetComponentsInChildren<Image>())
            part.FlipType = _Left ? ImageFlipType.VERTICAL : ImageFlipType.NONE;
    }
}