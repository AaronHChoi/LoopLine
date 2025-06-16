using UnityEngine;

namespace SoundSystem
{
    public class SoundBuilder
    {
        readonly SoundManager soundManager;
        SoundData soundData;
        Vector3 position = Vector3.zero;
        bool randomPitch;
        bool is3D;

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
            this.is3D = true;
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
            soundEmitter.With3D(is3D);

            if (randomPitch) soundEmitter.WithRandomPitch();

            soundEmitter.Play();
        }
        //If it's 3d, should add WithSoundPosition and automatically makes it 3d
        //Uncomment if need this without a position
        /*public SoundBuilder With3D()
        {
            this.is3D = true;
            return this;
        }*/
    }
}
