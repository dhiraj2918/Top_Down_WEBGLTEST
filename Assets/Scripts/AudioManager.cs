using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer; 
    public float defaultVolume = 0.5f;

    void Start()
    {
       
        SetVolume(defaultVolume);
    }

    public void SetVolume(float volume)
    {

        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }
}

