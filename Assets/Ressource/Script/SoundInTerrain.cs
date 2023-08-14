using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInTerrain : MonoBehaviour
{
    private AudioSource backgroundSound;
    private float startVolume;

    private void Start()
    {
        backgroundSound = GetComponent<AudioSource>();
        startVolume = backgroundSound.volume;
    }

    private void Update()
    {
        backgroundSound.volume = startVolume * PlayerPrefs.GetFloat("backsound");
    }
}
