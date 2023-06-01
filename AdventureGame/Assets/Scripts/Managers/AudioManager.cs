using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AudioManager manage and plays the audioclips of the game
/// </summary>
public class AudioManager : MonoBehaviour
{
    public AudioSource AudioSourceMusic;
    public AudioSource AudioSourceSFX;

    public bool Initialize()
    {
        AudioSourceMusic = transform.GetChild(0).GetComponent<AudioSource>();
        AudioSourceSFX = transform.GetChild(1).GetComponent<AudioSource>();


        if (AudioSourceMusic != null && AudioSourceSFX != null)
        {
            return true;
        }
        else 
        {
            Debug.LogWarning(gameObject.name + "Failed Initialization, one or more AudioSource is null");
            return false;
        }
    }
}
