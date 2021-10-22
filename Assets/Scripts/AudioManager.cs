using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //SOUNDS IN THE GAME
    public Sound[] sounds;

    //IS GAME SOUNDS MUTED
    [HideInInspector]
    public bool isMuted = false;

    //INSTANCE
    public static AudioManager Instance;

    //POSSIBLE MUTES STATS
    public enum MuteStat
    {
        Muted,
        Unmuted
    };


    void Awake()
    {
        //SET INSTANCE
        Instance = this;   
        
        //SET SOUND SETTINGS
        foreach(Sound s in sounds)
        {
           s.source =  gameObject.AddComponent<AudioSource>();
           s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
    }


    //PLAYS A SOUND
    public void Play(string name)
    {
        if(!isMuted)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
                return;
            s.source.Play();
        }
    }

    
    //CHANGES THE MUTE STATUS
    public void MuteControl(MuteStat stat)
    {
        switch (stat)
        {
            //STOP ALL SOUNDS and MUTE THEM
            case MuteStat.Muted:
                foreach (AudioSource source in transform.GetComponents<AudioSource>())
                {
                    source.mute = true;
                    source.Stop();
                    isMuted = true;
                }
                break;

            //UNMUTE ALL THE SOUND SOURCES
            case MuteStat.Unmuted:
                foreach (AudioSource source in transform.GetComponents<AudioSource>())
                {
                    source.mute = false;
                    isMuted = false;
                }
                break;
        }
    }
}
