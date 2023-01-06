using UnityEngine;
using UnityEngine.SceneManagement;
using VN;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_Text;
    float     m_SpiderSpawnedTime = 0;
    Hive      m_Hive;
    BeePlayer m_Player;

    void Awake()
    {
        m_Hive   = Hive.Create(null, Vector2.zero, "hive");
        m_Player = BeePlayer.Create(null, Vector2.zero, "BeePlayer", Vector2.zero);
    }

    public void IncresePoints()
    {
        m_Text.text = (int.Parse(m_Text.text) + 1).ToString();
    }

    void Update()
    {
        if (Time.time - m_SpiderSpawnedTime > 5)
        {
            Spider.Create(null, Utility.RandomGroundOffset, "spider");
            m_SpiderSpawnedTime = Time.time;
        }

        if (m_Player == null || m_Hive == null)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}