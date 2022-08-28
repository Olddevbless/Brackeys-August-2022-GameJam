using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationObject
{
    public Animation animation;
    public string id = "DEFAULT_ID";
}
public class AnimationRegistry : MonoBehaviour
{
    public AnimationObject[] animations;
    
    public static Dictionary<string, AnimationObject> RegisteredAnimations = new Dictionary<string, AnimationObject>();

    private void Awake()
    {
        foreach (var animation in animations)
        {
            RegisterAnimation(animation);
        }
    }

    public static void RegisterAnimation(AnimationObject obj)
    {
        RegisteredAnimations[obj.id] = obj;
    }
    public static void PlayAnimation(string id)
    {
        if (!RegisteredAnimations.ContainsKey(id))
        {
            Debug.LogWarning("Skipping unregistered animation with ID: " + id);
            return;
        }

        Animation animation = RegisteredAnimations[id].animation;
        
        if (animation.isPlaying) return;
        animation.Play();
    }
}
