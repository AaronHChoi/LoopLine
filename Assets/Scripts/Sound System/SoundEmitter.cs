using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SoundSystem
{
    public class SoundEmitter : MonoBehaviour
    {
        private AudioSource audioSource;
        private Coroutine playingCoroutine;

        private void Awake()
        {
            audioSource = gameObject.GetOrAdd<AudioSource>();
        }
        public void Initialize(SoundData data)
        {
            audioSource.clip = data.clip;
            audioSource.outputAudioMixerGroup = data.mixerGroup;
            audioSource.loop = data.isALoop;
            audioSource.playOnAwake = data.isAPlayOnAwake;
            audioSource.volume = data.volume;
        }
        public void Play()
        {
            if (playingCoroutine != null)
            {
                StopCoroutine(playingCoroutine);
            }

            audioSource.Play();
            playingCoroutine = StartCoroutine(nameof(WaitForSoundToEnd));
        }
        public void Stop()
        {
            if (playingCoroutine != null)
            {
                StopCoroutine(playingCoroutine);
                playingCoroutine = null;
            }
            audioSource.Stop();
            SoundManager.Instance.ReturnToPool(this);
        }

        IEnumerator WaitForSoundToEnd()
        {
            yield return new WaitWhile(() => audioSource.isPlaying);
            SoundManager.Instance.ReturnToPool(this);
        }
        internal void With3D(bool is3D)
        {
            audioSource.spatialBlend = is3D ? 1 : 0;
        }
        internal void WithRandomPitch(float min = -0.05f, float max = 0.05f)
        {
            audioSource.pitch += Random.Range(min, max);
        }
    }
}
