using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private List<AudioSource> sounds = new List<AudioSource>();
    private AudioSource soundAttack;

    private void Awake()
    {
        if(instance!=null)
        {
            return;
        }
        instance = this;
        GetSound();
        soundAttack = GetComponent<AudioSource>();
    }

    public void SetSoundAttack(AudioClip clip)
    {
        if(clip !=null && PlayerPrefs.GetInt("soundEffect")==0 && soundAttack!=null)
        {
            soundAttack.clip = clip;
            soundAttack.Play();
        }
        
    }

    private void GetSound()
    {
        foreach(Transform child in transform)
        {
            sounds.Add(child.GetComponent<AudioSource>());
        }
    }

    public void Sound(int idSound)
    {
        if(PlayerPrefs.GetInt("soundEffect")==0)
        {
            if(!sounds[idSound].loop)
            {
                sounds[idSound].Play();
            }
            else if(!sounds[idSound].isPlaying)
            {
                sounds[idSound].Play();
            }
        }
        
    }

    public void StopSound(int idSound)
    {
        sounds[idSound].Stop();
    }
}
