using UnityEngine;
using UnityEngine.SceneManagement;
using VN;

public class GameManager : MonoBehaviour
{
    float m_SpiderSpawnedTime = 0;
    Hive  m_Hive;

    void Awake()
    {
        m_Hive = Hive.Create(null, Vector2.zero, "hive");
    }

    void Update()
    {
        if (Time.time - m_SpiderSpawnedTime > 5)
        {
            Spider.Create(null, Utility.RandomOffset, "spider");
            m_SpiderSpawnedTime = Time.time;
        }
    }
}