using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Sound_System
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