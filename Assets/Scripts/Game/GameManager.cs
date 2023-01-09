using UnityEngine;
using UnityEngine.SceneManagement;
using VN;
using TMPro;

public class GameManager : MonoBehaviour
{
    float SPIDER_SPAWN_INTERVAL = 5f;
    [SerializeField] TextMeshProUGUI m_Text;
    [SerializeField] Node            m_Root;

    float       m_SpiderSpawnedTime = 0;
    Hive        m_Hive;
    BeePlayer   m_Player;
    static GameManager m_Instance;

    public static GameManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<GameManager>();

            return m_Instance;
        }
    }

    public Node Root => m_Root;

    void Awake()
    {
        m_Root.LocalRect = new Rect(-Utility.ScreenSize/2, Utility.ScreenSize);

        m_Hive   = CharacterManager.Instance.Create<Hive>("hive", null, new Rect(0, 0, 2, 2));
        m_Player = CharacterManager.Instance.Create<BeePlayer>("BeePlayer", null, new Rect(0, 0, 1, 1));
    }

    void Update()
    {
        if (Time.time - m_SpiderSpawnedTime > SPIDER_SPAWN_INTERVAL)
        {
            CharacterManager.Instance.Create<Spider>("spider", null, CharacterManager.RandomGroundRect);
            m_SpiderSpawnedTime = Time.time;
        }

        if (m_Player == null || m_Hive == null)
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}