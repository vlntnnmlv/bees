using UnityEngine;
using VN;

public class Spider : Character
{
    #region creation

    public static Spider Create(Node _Parent, Vector2 _Offset, string _ID, float _Health = 25, float _Speed = 1, float _Damage = 4)
    {
        Spider spider = Utility.LoadObject<Spider>("Prefabs/Spider", _ID, _Parent);

        spider.Create(_Offset, _Health, _Speed, _Damage);
        return spider;
    }

    #endregion

    #region properties

    public override GroupType Group => GroupType.HOSTILE;

    #endregion

    #region service methods

    protected override void OnUpdate()
    {
        Direction = -Offset.normalized;

        base.OnUpdate();

        foreach (SpriteAnimated sprite in GetComponentsInChildren<SpriteAnimated>())
            sprite.Playing = (Direction - Vector2.zero).sqrMagnitude > 0.001f;
    }

    #endregion

}