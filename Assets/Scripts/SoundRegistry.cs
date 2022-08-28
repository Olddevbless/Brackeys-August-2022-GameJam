using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRegistry : MonoBehaviour
{
    public AudioObject[] audioObjects;

    private void Awake()
    {
        foreach (var audioObject in audioObjects)
        {
            SoundManager.RegisterSound(audioObject);
        }
    }
}