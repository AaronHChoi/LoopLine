using UnityEngine;
using SoundSystem;

public class AdditiveBGM : MonoBehaviour
{
    [SerializeField] SoundData bgmData;

    private SoundEmitter soundEmitted;
    private void Start()
    {
        soundEmitted =
            SoundManager.Instance.CreateSound()
            .WithSoundData(bgmData)
            .Play();
    }
    private void OnDisable()
    {
        if (soundEmitted != null)
        {
            soundEmitted.Stop();
        }
    }
}
