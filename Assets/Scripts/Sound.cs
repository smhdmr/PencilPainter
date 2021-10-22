using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound 
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [HideInInspector]
    public bool loop = false;
    [HideInInspector]
    public float pitch = 1f;
    [HideInInspector]
    public bool playOnAwake = false;
    [HideInInspector]
    public AudioSource source;
}
