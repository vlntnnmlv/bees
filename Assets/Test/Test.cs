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

    [SerializeField] Node m_Root;

    [ExecuteAlways]
    void Start()
    {
        m_Root.LocalRect = new Rect(-Utility.ScreenSize/2, Utility.ScreenSize);

        Node.Create("test", m_Root, new Rect(0, 0, 3.3f, 3.3f));
    }

    void OnGUI()
    {
        if (Button(0, 0, "Spawn Bee Worker"))
            CharacterManager.Instance.Create<BeeWorker>("bee", null, new Rect(Vector2.zero, Vector2.one));
        if (Button(0, 1, "Spawn Bee Player") && m_Player == null)
            m_Player = CharacterManager.Instance.Create<BeePlayer>("bee", m_Root, new Rect(Vector2.zero, Vector2.one));
        if (Button(0, 2, "Spawn Spider"))
            Spider.Create("spider", null, new Rect(Utility.RandomOffset, Vector2.one));

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
                character.Health = 0;
            }
        }

        if (Button(3, 0, "Test hit effects"))
        {
            m_Player = CharacterManager.Instance.Create<BeePlayer>("bee", m_Root, new Rect(Vector2.zero, Vector2.one));
            m_TestingHitEffects = true;
        }
    }

    void Update()
    {
        if (m_TestingHitEffects)
        {
            if (Time.time - m_SpiderSpawnedTime > 2)
            {
                Spider.Create("spider", null, new Rect(Vector2.up * 5, Vector2.one));
                m_SpiderSpawnedTime = Time.time;
            }
        }
    }

    bool Button(int _Col, int _Row, string _Text)
    {
        return GUI.Button(new Rect(OFFSET + (100 + MARGIN) * _Col, OFFSET + (MARGIN + 100) * _Row, 100, 100), _Text);
    }
}