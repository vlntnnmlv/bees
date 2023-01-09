using UnityEngine;
using System;

namespace VN
{

[CreateAssetMenu(fileName = "Texture", menuName = "ScriptableObjects/SpawnCharacterConfig", order = 1)]
[Serializable]
public class CharacterConfig : ScriptableObject
{
    public float  Health;
    public float  Speed;
    public float  Damage;
    public string PrefabName;
}

}