using UnityEngine;
using UnityEditor;
using VN;

public class Test : MonoBehaviour
{
    const int OFFSET = 50;
    const int MARGIN = 5;

    BeePlayer m_Player;
    bool      m_TestingHitEffects;
    float     m_SpiderSpawnedTime;

    void OnGUI()
    {
        if (Button(0, 0, "Spawn Bee Worker"))
            BeeWorker.Create(null, Utility.RandomOffset, "bee", Utility.RandomOffset);
        if (Button(0, 1, "Spawn Bee Player") && m_Player == null)
            m_Player = BeePlayer.Create(null, Vector2.zero, "bee", Utility.RandomOffset);
        if (Button(0, 2, "Spawn Spider"))
            Spider.Create(null, Utility.RandomOffset, "spider");

        if (Button(1, 0, "Decrease Bee Health") && m_Player != null)
            m_Player.Health -= 10;
        if (Button(1, 1, "Increase Bee Health") && m_Player != null)
            m_Player.Health += 10;

        if (Button(2, 0, "Clear"))
        {
            m_TestingHitEffects = false;
            Character[] characters = FindObjectsOfType<Character>();
            foreach (Character character in characters)
            {
                Destroy(character.gameObject);
                Destroy(character);
            }
        }

        if (Button(3, 0, "Test hit effects"))
        {
            m_Player = BeePlayer.Create(null, Vector2.zero, "bee", Vector2.zero);

            m_TestingHitEffects = true;
        }
    }

    void Update()
    {
        if (m_TestingHitEffects)
        {
            if (Time.time - m_SpiderSpawnedTime > 2)
            {
                Spider.Create(null, new Vector2(0, 5), "spider");
                m_SpiderSpawnedTime = Time.time;
            }
        }
    }

    bool Button(int _Col, int _Row, string _Text)
    {
        return GUI.Button(new Rect(OFFSET + (100 + MARGIN) * _Col, OFFSET + (MARGIN + 100) * _Row, 100, 100), _Text);
    }
}