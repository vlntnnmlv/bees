using UnityEngine;
using UnityEditor;
using VN;

public class Test : MonoBehaviour
{
    const int OFFSET = 50;
    const int MARGIN = 5;

    BeePlayer player;
    void OnGUI()
    {
        if (Button(0, 0, "Spawn Bee Worker"))
            BeeWorker.Create(null, Vector2.zero, "bee", Utility.RandomOffset);
        if (Button(0, 1, "Spawn Bee Player") && player == null)
            player = BeePlayer.Create(null, Vector2.zero, "bee", Utility.RandomOffset);
        if (Button(0, 2, "Spawn Spider"))
            Spider.Create(null, Utility.RandomOffset, "spider");

        if (Button(1, 0, "Decrease Bee Health") && player != null)
            player.Health -= 10;
        if (Button(1, 1, "Increase Bee Health") && player != null)
            player.Health += 10;
    }

    bool Button(int _Col, int _Row, string _Text)
    {
        return GUI.Button(new Rect(OFFSET + (100 + MARGIN) * _Col, OFFSET + (MARGIN + 100) * _Row, 100, 100), _Text);
    }
}