using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
    public string tag;

    [HideInInspector] public List<AudioSource> sources = new();

    public AudioClip clip;

    // [Range(0f, 1f)]
    // public float volume;

    [Range(0f, 1f)]
    public float minVolume = 0.4f;
    [Range(0f, 1f)]
    public float maxVolume = 0.6f;

    // [Range(.1f, 3f)]
    // public float pitch;
    [Range(.1f, 3f)]
    public float minPitch = 1f;
    [Range(.1f, 3f)]
    public float maxPitch = 1.5f;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}