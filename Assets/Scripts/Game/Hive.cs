using UnityEngine;
using System.Collections.Generic;
using VN;

public class Hive : Image
{
    #region constants
    const float POT_ICON_MARGIN = 0.1f;

    #endregion

    int m_BeesCount = 1;
    int m_HoneyPotsCount = 0;

    float m_TimeFlowerSpawned = 0;
    float PeriodFlowerSpawn => 17 - 0.05f * Mathf.Sqrt(m_BeesCount);

    List<HoneyPot> m_HoneyPotIcons = new List<HoneyPot>();

    new public static Hive Create(Node _Parent, Vector2 _Offset, string _ID)
    {
        Hive hive = Utility.LoadObject<Hive>("Prefabs/Hive", _ID, _Parent);
        hive.Create(_Offset);

        return hive;
    }

    void Start()
    {
        BeePlayer.Create(null, Vector2.zero, "BeePLayer", Vector2.zero);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (Time.time - m_TimeFlowerSpawned > PeriodFlowerSpawn)
        {
            Flower.Create(null, VN.Utility.RandomOffset, "flower");
            m_TimeFlowerSpawned = Time.time;
        }

        Bee[] bees = FindObjectsOfType<Bee>();
        foreach (Bee bee in bees)
        {
            if (Vector2.Distance(bee.Offset, Offset) < 1 && bee.GotHoney)
            {
                bee.DropHoneyPot(Offset);
                m_HoneyPotsCount += 1;
                AddHoneyPotsIcon();
                if (m_HoneyPotsCount == 5)
                {
                    BeeWorker.Create(this, Offset, $"bee_worker_{m_BeesCount}", VN.Utility.RandomOffset);
                    m_BeesCount += 1;
                    ClearHoneyPotIcons();
                }

                Flower.Create(null, VN.Utility.RandomOffset, "flower");
            }
        }
    }

    void AddHoneyPotsIcon()
    {
        HoneyPot potIcon = HoneyPot.Create(this, Utility.TopLeftCornerOffset + Vector2.right * POT_ICON_MARGIN * m_HoneyPotsCount, "icon");
        m_HoneyPotIcons.Add(potIcon);
    }

    void ClearHoneyPotIcons()
    {
        m_HoneyPotsCount = 0;
        foreach (HoneyPot icon in m_HoneyPotIcons)
            StartCoroutine(icon.Disappear());
        m_HoneyPotIcons.Clear();
    }
}