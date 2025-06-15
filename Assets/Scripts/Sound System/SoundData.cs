using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundSystem
{
    [Serializable]
    public class SoundData
    {
        public AudioClip clip;
        public AudioMixerGroup mixerGroup;
        public bool isALoop;
        public bool isAPlayOnAwake;
    }
}