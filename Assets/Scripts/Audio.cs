using UnityEngine.Audio;
using UnityEngine;
[System.Serializable]
public class Audio
{
    public AudioClip clip;
    public string name;
    public float volume;
    public float pitch;
    public bool loop;
    [HideInInspector]
    public AudioSource source;
}