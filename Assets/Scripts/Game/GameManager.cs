using UnityEngine;
using VN;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_Text;
    float m_SpiderSpawnedTime = 0;
    Hive  m_Hive;

    void Awake()
    {
        m_Hive = Hive.Create(null, Vector2.zero, "hive");
    }

    public void IncresePoints()
    {
        m_Text.text = (int.Parse(m_Text.text) + 1).ToString();
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