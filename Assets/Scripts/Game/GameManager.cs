using UnityEngine;
using VN;

public class GameManager : MonoBehaviour
{
    float m_SpiderSpawnedTime = 0;

    void Update()
    {
        if (Time.time - m_SpiderSpawnedTime > 5)
        {
            Spider.Create(null, Utility.RandomOffset, "spider");
            m_SpiderSpawnedTime = Time.time;
        }
    }
}