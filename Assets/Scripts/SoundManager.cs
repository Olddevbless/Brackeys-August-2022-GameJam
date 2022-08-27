using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class AudioObject
{
    public AudioSource sound;
    public string identifier;

    public AudioObject(string id, AudioSource soundData)
    {
        this.identifier = id;
        this.sound = soundData;
        Create(id, soundData);
    }
    
    public static void Create(string id,  AudioSource soundData)
    {
        SoundManager.RegisterSound(new AudioObject(id, soundData));
    }
}

public static class SoundManager
{
    private static Dictionary<string, AudioObject> _registeredSounds = new Dictionary<string, AudioObject>();

    public static void RegisterSound(AudioObject sound)
    {
        _registeredSounds.Add(sound.identifier, sound);
    }
    
    public static void PlaySound(string identifier)
    {
        AudioSource sound = _registeredSounds[identifier].sound;
        
        if (sound.isPlaying) return;
        
        sound.Play();
    }
}