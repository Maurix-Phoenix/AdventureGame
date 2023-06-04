using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static MXUtilities;

/// <summary>
/// AudioManager manage and plays the audioclips of the game
/// </summary>
public class AudioManager : MonoBehaviour
{
    public AudioSource AudioSourceMusic;
    public AudioSource AudioSourceSFX;

    public List<Music> Musics= new List<Music>();
    public List<SFX> SFXs = new List<SFX>();

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





    public void PlayMusic(string clipName)
    {
        foreach (Music music in Musics)
        {
            if (music != null)
            {
                if (music.Name == clipName)
                {
                    AudioSourceMusic.clip = music.clip;
                    AudioSourceMusic.Play();
                    return;
                }
                MXDebug.Log($"[AudioManager]: can't find clip named '{clipName}' in the Music list.", MXUtilities.MXDebug.LogType.Warning);
            }
            else MXDebug.Log($"[AudioManager]: AudioClip invalid.", MXUtilities.MXDebug.LogType.Warning);
        }
    }
    public void PlayMusic(AudioClip ac)
    {//global music
        foreach (Music music in Musics)
        {
            if (music != null)
            {
                if (music.clip == ac)
                {
                    AudioSourceMusic.clip = ac;
                    AudioSourceMusic.Play();
                    return;
                }
                MXDebug.Log($"[AudioManager]: can't find clip in the Musics list.", MXUtilities.MXDebug.LogType.Warning);
            }
            else MXDebug.Log($"[AudioManager]: AudioClip invalid.", MXUtilities.MXDebug.LogType.Warning);
        }
    }

    public void PlaySFX(string clipName)
    {
        foreach (SFX sfx in SFXs)
        {
            if (sfx != null)
            {
                if (sfx.Name == clipName)
                {
                    AudioSourceSFX.PlayOneShot(sfx.clip);
                    return;
                }
                MXDebug.Log($"[AudioManager]: can't find clip named '{clipName}' in the Music list.", MXUtilities.MXDebug.LogType.Warning);
            }
            else MXDebug.Log($"[AudioManager]: AudioClip invalid.", MXUtilities.MXDebug.LogType.Warning);
        }
    }
    public void PlaySFX(AudioClip ac)
    {//global sound effect
        foreach(SFX sfx in SFXs)
        {
            if (sfx != null)
            {
                if(sfx.clip == ac)
                {
                    AudioSourceSFX.PlayOneShot(ac);
                    return;
                }
                MXDebug.Log($"[AudioManager]: can't find clip in the SFXs list.", MXUtilities.MXDebug.LogType.Warning);
            }
            else MXDebug.Log($"[AudioManager]: AudioClip invalid.", MXUtilities.MXDebug.LogType.Warning);
        }
    }



    public void PlayMusicLocal(AudioSource audioS, string clipName)
    {//local music
        if (audioS != null && clipName != null)
        {
            foreach (Music music in Musics)
            {
                if (music != null)
                {
                    if (music.Name == clipName)
                    {
                        audioS.clip = music.clip;
                        audioS.Play();
                        return;
                    }
                }
                MXDebug.Log($"[AudioManager]: can't find clip named '{clipName}' in the Music list.", MXUtilities.MXDebug.LogType.Warning);
            }
        }
        else MXDebug.Log($"[AudioManager]: Audiosource or clip invalid in {audioS.gameObject.name}.", MXUtilities.MXDebug.LogType.Warning);
    }
    public void PlayMusicLocal(AudioSource audioS, AudioClip ac)
    {//local music
        if (audioS != null && ac != null)
        {
            foreach (Music music in Musics)
            {
                if (music != null)
                {
                    if (music.clip == ac)
                    {
                        audioS.clip = ac;
                        audioS.Play();
                        return;
                    }
                }
                MXDebug.Log($"[AudioManager]: can't find clip {ac.name} in the Music list.", MXUtilities.MXDebug.LogType.Warning);
            }
        }
        else MXDebug.Log($"Audiosource or clip invalid in {audioS.gameObject.name}.", MXUtilities.MXDebug.LogType.Warning);
    }

    public void PlaySFXLocal(AudioSource audioS, string clipName)
        {//local music
        if (audioS != null && clipName != null)
        {
            foreach (SFX sfx in SFXs)
            {
                if (sfx != null)
                {
                    if (sfx.Name == clipName)
                    {
                        audioS.PlayOneShot(sfx.clip);
                        return;
                    }
                }
                MXDebug.Log($"[AudioManager]: can't find clip named '{clipName}' in the SFXs list.", MXUtilities.MXDebug.LogType.Warning);
            }
        }
        else MXDebug.Log($"Audiosource or clip invalid in {audioS.gameObject.name}.", MXUtilities.MXDebug.LogType.Warning);
    }
    public void PlayLocalSFXLocal(AudioSource audioS, AudioClip ac)
    {//local sound effect
        if (audioS != null && ac != null)
        {
            foreach (SFX sfx in SFXs)
            {
                if (sfx != null)
                {
                    if (sfx.clip == ac)
                    {
                        audioS.PlayOneShot(ac);
                        return;
                    }
                }
                MXDebug.Log($"[AudioManager]: can't find clip {ac.name} in the SFXs list.", MXUtilities.MXDebug.LogType.Warning);
            }
        }
        else MXDebug.Log($"Audiosource or clip invalid in {audioS.gameObject.name}.", MXUtilities.MXDebug.LogType.Warning);
    }


    [Serializable]
    public class Music
    {
        public string Name;
        public AudioClip clip;
    }

    [Serializable]
    public class SFX
    {
        public string Name;
        public AudioClip clip;
    }
}
