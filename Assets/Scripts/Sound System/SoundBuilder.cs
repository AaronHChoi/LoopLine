using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SoundSystem
{
    public class SoundBuilder
    {
        readonly SoundManager soundManager;
        SoundData soundData;
        Vector3 position = Vector3.zero;
        bool randomPitch;

        public SoundBuilder(SoundManager soundManager)
        {
            this.soundManager = soundManager;
        }

        public SoundBuilder WithSoundData(SoundData soundData)
        {
            this.soundData = soundData;
            return this;
        }
        public SoundBuilder WithSoundPosition(Vector3 position)
        {
            this.position = position;
            return this;
        }
        public SoundBuilder WithRandomPitch()
        {
            this.randomPitch = true;
            return this;
        }
        public void Play()
        {
            SoundEmitter soundEmitter = soundManager.Get();
            soundEmitter.Initialize(soundData);
            soundEmitter.transform.position = position;
            soundEmitter.transform.parent = SoundManager.Instance.transform;

            if (randomPitch)
            {
                soundEmitter.WithRandomPitch();
            }

            soundEmitter.Play();
        }
    }
}
