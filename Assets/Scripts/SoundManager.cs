using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private static Dictionary<string, AudioObject> _registeredSounds = new Dictionary<string, AudioObject>();

    public static void RegisterSound(AudioObject sound)
    {
        _registeredSounds.Add(sound.identifier, sound);
        Debug.Log("Registering sound with ID: " + sound.identifier);
    }
    
    public static void PlaySound(string identifier)
    {
        AudioSource sound = _registeredSounds[identifier].sound;
        
        if (sound.isPlaying) return;
        
        sound.Play();
    }
}