using UnityEngine;
using VN;

public class Hive : Image
{
    int i = 0;

    public static Hive Create(Node _Parent, Vector2 _Offset, string _ID)
    {
        Hive hive = Utility.LoadObject<Hive>("Prefabs/Hive", _ID, _Parent);
        hive.Create(_Offset);

        return hive;
    }

    void Update()
    {
        Bee[] bees = FindObjectsOfType<Bee>();
        foreach (Bee bee in bees)
        {
            if (Vector2.Distance(bee.Offset, Offset) < 1 && bee.GotAPot)
            {
                bee.DropPot(Offset);
                Bee.Create(null, Offset, $"bee_{i}", VN.Utility.RandomOffset);
                i += 1;
                HoneyPot.Create(null, VN.Utility.RandomOffset, "honey_pot");
            }
        }
    }
}