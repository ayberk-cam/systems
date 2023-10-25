using UnityEngine.Audio;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Sound[] inGameSFX;
    public Sound[] inGameMusics;
    public Sound[] mainMenuMusics;
    public Sound[] mainMenuSFX;

    private List<Sound> allSounds = new();

    public static SoundManager instance;

    private string LastMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);

            return;
        }

        SetupSounds();
    }

    private void SetupSounds()
    {
        foreach (Sound s in inGameSFX)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.sources.Add(s.source);

            s.source.clip = s.clip;
            // s.source.volume = s.volume;
            // s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            allSounds.Add(s);
        }

        foreach (Sound m in inGameMusics)
        {
            m.source = gameObject.AddComponent<AudioSource>();

            m.sources.Add(m.source);

            m.source.clip = m.clip;
            // m.source.volume = m.volume;
            // m.source.pitch = m.pitch;
            m.source.loop = m.loop;

            allSounds.Add(m);
        }

        foreach (Sound m in mainMenuSFX)
        {
            m.source = gameObject.AddComponent<AudioSource>();

            m.sources.Add(m.source);

            m.source.clip = m.clip;
            // m.source.volume = m.volume;
            // m.source.pitch = m.pitch;
            m.source.loop = m.loop;

            allSounds.Add(m);
        }

        foreach (Sound m in mainMenuMusics)
        {
            m.source = gameObject.AddComponent<AudioSource>();

            m.sources.Add(m.source);

            m.source.clip = m.clip;
            // m.source.volume = m.volume;
            // m.source.pitch = m.pitch;
            m.source.loop = m.loop;

            allSounds.Add(m);
        }
    }

    public void Play(string soundTag)
    {
        Sound s = allSounds.Find(sound => sound.tag == soundTag);

        switch(soundTag)
        {
            case "InGame":
                LastMusic = "InGame";
                break;
            case "Boss":
                LastMusic = "Boss";
                break;
            case "MainMenu":
                LastMusic = "MainMenu";
                break;
        }


        if (s == null)
        {
            Debug.LogWarning("the sound " + soundTag + " doesnot exist!");

            return;
        }

        if (s.source.isPlaying && !s.loop)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();

            s.sources.Add(source);

            source.clip = s.clip;

            // source.volume = s.volume;
            source.volume = s.source.volume;
            source.pitch = s.source.pitch;

            source.Play();

            DisableAudioSource(source, s);
        }
        else
        {
            if (s.source.volume != 0)
            {
                s.source.volume = UnityEngine.Random.Range(s.minVolume, s.maxVolume);
                s.source.pitch = UnityEngine.Random.Range(s.minPitch, s.maxPitch);
            }

            s.source.time = 0f;
            s.source.Play();
        }
    }

    public void Pause(string soundTag)
    {
        Sound s = allSounds.Find(sound => sound.tag == soundTag);

        if (s == null)
        {
            Debug.LogWarning("the sound " + soundTag + " doesnot exist!");

            return;
        }

        s.source.Pause();
    }

    public float GetSoundLength(string soundTag)
    {
        Sound s = allSounds.Find(sound => sound.tag == soundTag);

        float length = s.source.clip.length;

        return length;
    }

    public void EnableInGameSFXs()
    {
        foreach (Sound s in inGameSFX)
        {
            foreach (AudioSource source in s.sources)
            {
                source.volume = UnityEngine.Random.Range(s.minVolume, s.maxVolume);
                source.pitch = UnityEngine.Random.Range(s.minPitch, s.maxPitch);
            }
        }
    }

    public void EnableMainMenuSFXs()
    {
        foreach (Sound s in mainMenuSFX)
        {
            foreach (AudioSource source in s.sources)
            {
                source.volume = UnityEngine.Random.Range(s.minVolume, s.maxVolume);
                source.pitch = UnityEngine.Random.Range(s.minPitch, s.maxPitch);
            }
        }
    }

    public void DisableInGameSFXs()
    {
        foreach (Sound s in inGameSFX)
        {
            foreach (AudioSource source in s.sources)
            {
                source.volume = 0;
                source.pitch = 0;
            }
        }
    }

    public void DisableMainMenuSFXs()
    {
        foreach (Sound s in mainMenuSFX)
        {
            foreach (AudioSource source in s.sources)
            {
                source.volume = 0;
                source.pitch = 0;
            }
        }
    }

    public void EnableInGameMusics()
    {
        foreach (Sound m in inGameMusics)
        {
            foreach (AudioSource source in m.sources)
            {
                source.volume = UnityEngine.Random.Range(m.minVolume, m.maxVolume);
                source.pitch = UnityEngine.Random.Range(m.minPitch, m.maxPitch);
            }
        }
    }

    public void EnableMainMenuMusics()
    {
        foreach (Sound m in mainMenuMusics)
        {
            foreach (AudioSource source in m.sources)
            {
                source.volume = UnityEngine.Random.Range(m.minVolume, m.maxVolume);
                source.pitch = UnityEngine.Random.Range(m.minPitch, m.maxPitch);
            }
        }
    }

    public void DisableInGameMusics()
    {
        foreach (Sound m in inGameMusics)
        {
            foreach (AudioSource source in m.sources)
            {
                source.volume = 0;
                source.pitch = 0;
            }
        }
    }

    public void DisableMainMenuMusics()
    {
        foreach (Sound m in mainMenuMusics)
        {
            foreach (AudioSource source in m.sources)
            {
                source.volume = 0;
                source.pitch = 0;
            }
        }
    }

    private void DisableAudioSource(AudioSource source, Sound s)
    {
        s.sources.Remove(source);

        Destroy(source, GetSoundLength(s.tag));
    }

    public void PlayLastMusic()
    {
        //Debug.Log(LastMusic);
        Play(LastMusic);
    }

    public void StopLastMusic()
    {
        Pause(LastMusic);
    }
}
