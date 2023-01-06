using UnityEngine;

public static class SoundMaker
{
   public static void PlaySound(string _Sound, Vector2 _Pos, bool _Loop = false)
    {
        GameObject  sound  = new GameObject(_Sound);
        AudioSource source = sound.AddComponent<AudioSource>();
        AudioClip   clip   = Resources.Load<AudioClip>("Audio/" + _Sound);
        source.clip = clip;
        source.loop = _Loop;
        source.Play();
        Object.Destroy(sound, clip.length);
    }
}