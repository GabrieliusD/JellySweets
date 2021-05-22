using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Audio[] sounds;
    bool muted = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;

        foreach (Audio a in sounds)
        {
            a.source = gameObject.AddComponent<AudioSource>();
            a.source.clip = a.clip;
            a.source.volume = a.volume;
            a.source.pitch = a.pitch;
            a.source.loop = a.loop;
        }
    }

    private void Start()
    {
        Play("MainMenu");
    }

    public void Play(string name)
    {
        Audio a = Array.Find(sounds, sound => sound.name == name);
        a.source.Play();
    }
    public void Stop(string name)
    {
        Audio a = Array.Find(sounds, sound => sound.name == name);
        a.source.Stop();
    }

    public void MuteAll()
    {
        if(!muted)
        {
            foreach (var item in sounds)
            {
                item.source.volume = 0;
            }
            muted = true;
        }
        else
        {
            foreach (var item in sounds)
            {
                item.source.volume = item.volume;
            }
            muted = false;
        }
    }
}
