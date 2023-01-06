using UnityEngine;
using System.Collections.Generic;
using VN;

public class Hive : Character
{
    #region creation

    new public static Hive Create(Node _Parent, Vector2 _Offset, string _ID)
    {
        Hive hive = Utility.LoadObject<Hive>("Prefabs/Hive", _ID, _Parent);
        hive.Create(_Offset, 250, 0, 0);

        return hive;
    }

    #endregion

    #region constants
    const float POT_ICON_MARGIN = 0.3f;

    #endregion

    #region attributes

    int            m_BeesCount         = 1;
    int            m_HoneyPotsCount    = 0;
    float          m_TimeFlowerSpawned = 0;
    List<HoneyPot> m_HoneyPotIcons = new List<HoneyPot>();
    
    #endregion

    #region properties

    public override bool IsHealthBarActive => base.IsHealthBarActive && IsDamaged;
    public override GroupType Group        => GroupType.PEACEFULL;

    float PeriodFlowerSpawn => 17 - 0.05f * Mathf.Sqrt(m_BeesCount);

    #endregion

    #region engine methods

    void Start()
    {
        Flower.Create(null, VN.Utility.RandomGroundOffset, "flower");
        m_TimeFlowerSpawned = Time.time;
    }

    #endregion

    #region service methods

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (Time.time - m_TimeFlowerSpawned > PeriodFlowerSpawn)
        {
            Flower.Create(null, VN.Utility.RandomGroundOffset, "flower");
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

                Flower.Create(null, VN.Utility.RandomGroundOffset, "flower");
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

    #endregion
}