using System.Collections.Generic;
using UnityEngine;
using VN;

public class HealZone : Image
{
    List<Character> m_HealedCharacters = new List<Character>();

    protected override void OnUpdate()
    {
        base.OnUpdate();

        Character[] characters = FindObjectsOfType<Character>();
        foreach (Character character in characters)
        {
        }
    }

    void OnTriggerEnter(Collider _Collider)
    {
        Character character = _Collider.GetComponent<Character>();
        if (m_HealedCharacters.Contains(character))
            character.Health += 1;
        else if (character != null && character.Group == Character.GroupType.FRIENDLY)
            m_HealedCharacters.Add(character);
    }

    void OnTriggerExit(Collider _Collider)
    {
        Character character = _Collider.GetComponent<Character>();
        m_HealedCharacters.Remove(character);
    }
}