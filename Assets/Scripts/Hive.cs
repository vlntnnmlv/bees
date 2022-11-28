using UnityEngine;

public class Hive : VN.Object
{
    Vector2 RandomDiff => new Vector2(Random.Range(-2,2), Random.Range(-2, 2));
    int i = 0;

    public static void Create(string _ID, VN.Object _Parent, Vector2 _Offset)
    {
        Hive hive = ResourceManager.Load<Hive>("Prefabs/Hive", _ID);
        hive.Create(_Offset, _Parent);
    }

    void Update()
    {
        Bee[] bees = FindObjectsOfType<Bee>();
        foreach (Bee bee in bees)
        {
            if (Vector2.Distance(bee.Offset, Offset) < 1 && bee.GotAPot)
            {
                bee.DropPot(Offset);
                Bee.Create($"bee_{i}", null, Offset, Offset + RandomDiff);
                i += 1;
                HoneyPot.Create("honey_pot", null, VN.Utility.RandomOffset);
            } 
        }
    }
}