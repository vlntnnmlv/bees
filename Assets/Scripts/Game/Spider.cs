using UnityEngine;
using VN;

public class Spider : Character
{
    new public static Spider Create(Node _Parent, Vector2 _Offset, string _ID)
    {
        Spider spider = Utility.LoadObject<Spider>("Prefabs/Spider", _ID, _Parent);

        spider.Create(_Offset);
        spider.Health = 25;
        spider.Speed = 4;
        return spider;
    }

    protected override void OnUpdate()
    {

        Direction = -Offset.normalized;

        base.OnUpdate();
    }
}