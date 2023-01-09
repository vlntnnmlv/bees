using UnityEngine;
using System.Collections.Generic;
using VN;

public class Hive : Character
{
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

    protected override void Start()
    {
        CharacterManager.Instance.Create<Flower>("flower", null, CharacterManager.RandomGroundRect);

        m_TimeFlowerSpawned = Time.time;
    }

    #endregion

    #region service methods

    protected override void Update()
    {
        base.Update();

        if (Time.time - m_TimeFlowerSpawned > PeriodFlowerSpawn)
        {
            CharacterManager.Instance.Create<Flower>("flower", null, CharacterManager.RandomGroundRect);
            m_TimeFlowerSpawned = Time.time;
        }

        Bee[] bees = FindObjectsOfType<Bee>();
        foreach (Bee bee in bees)
        {
            if (Vector2.Distance(bee.Offset, Offset) < 1 && bee.GotHoney)
            {
                bee.DropHoneyPot(WorldRect.center);
                m_HoneyPotsCount += 1;
                AddHoneyPotsIcon();
                if (m_HoneyPotsCount == 5)
                {
                    CharacterManager.Instance.Create<BeeWorker>($"bee_worker_{m_BeesCount}", this, LocalRect);
                    m_BeesCount += 1;
                    ClearHoneyPotIcons();
                }

                CharacterManager.Instance.Create<Flower>("flower", null, CharacterManager.RandomGroundRect);
            }
        }
    }

    void AddHoneyPotsIcon()
    {
        HoneyPot potIcon = HoneyPot.Create(
                "icon",
                this,
                new Rect(Utility.TopLeftCornerOffset + Vector2.right * POT_ICON_MARGIN * m_HoneyPotsCount, Vector2.one)
            );
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