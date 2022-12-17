using UnityEngine;
using UnityEditor;
using VN;

public class Test : MonoBehaviour
{
    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "Spawn Bee Worker"))
            BeeWorker.Create(null, Vector2.zero, "bee", Utility.RandomOffset);
        if (GUI.Button(new Rect(0, 100, 100, 100), "Spawn Bee Player"))
            BeePlayer.Create(null, Vector2.zero, "bee", Utility.RandomOffset);
    }
}