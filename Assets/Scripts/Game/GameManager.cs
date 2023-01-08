using UnityEngine;
using UnityEngine.SceneManagement;
using VN;
using TMPro;

public class GameManager : MonoBehaviour
{
    float SPIDER_SPAWN_INTERVAL = 3.5f;
    [SerializeField] TextMeshProUGUI m_Text;
    [SerializeField] Image           m_Background;

    float     m_SpiderSpawnedTime = 0;
    Hive      m_Hive;
    BeePlayer m_Player;

    void Awake()
    {
        m_Background.LocalRect = new Rect(-Utility.Size/2, Utility.Size);

        m_Hive   = Hive.Create("hive", null, new Rect(0, 0, 1, 1));
        m_Player = BeePlayer.Create("BeePlayer", null, new Rect(0, 0, 1, 1), Vector2.zero);
    }

    void Update()
    {
        if (Time.time - m_SpiderSpawnedTime > SPIDER_SPAWN_INTERVAL)
        {
            Spider.Create("spider", null, new Rect(Utility.RandomGroundOffset, Vector2.one));
            m_SpiderSpawnedTime = Time.time;
        }

        if (m_Player == null || m_Hive == null)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}