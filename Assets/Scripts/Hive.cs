using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using VN;

public class Hive : Image
{
    const float POT_ICON_MARGIN = 0.1f;
    int m_BeesCount = 0;
    int m_HoneyPotsCount = 0;

    List<HoneyPot> m_HoneyPotIcons = new List<HoneyPot>();

    public static Hive Create(Node _Parent, Vector2 _Offset, string _ID)
    {
        Hive hive = Utility.LoadObject<Hive>("Prefabs/Hive", _ID, _Parent);
        hive.Create(_Offset);

        return hive;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

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
        StartCoroutine(ClearHoneyPotIconsCoroutine());
    }

    IEnumerator ClearHoneyPotIconsCoroutine()
    {
        foreach (HoneyPot icon in m_HoneyPotIcons)
        {
            StartCoroutine(icon.Disappear());
            yield return new WaitForSeconds(0.15f);
        }
        m_HoneyPotIcons.Clear();
    }
}