using System;
using UnityEngine;
using UnityEngine.Audio;


[Serializable]
public class SoundData
{
    public AudioClip clip;
    public AudioMixerGroup mixerGroup;
    public bool isALoop;
    public bool isAPlayOnAwake;
    [Range(0f,1f)] public float volume = 1f;
    public float secondsDelay = 0;
}
