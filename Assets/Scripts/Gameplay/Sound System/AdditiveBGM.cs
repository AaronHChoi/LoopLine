using UnityEngine;
using Core.Audio;
using Core.DependencyInjection;

public class AdditiveBGM : MonoBehaviour
{
    [SerializeField] SoundData bgmData;

    ISoundEmitter soundEmitted;
    ISoundManager soundManager;

    private void Awake()
    {
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();    
    }
    private void Start()
    {
        soundEmitted =
            soundManager.CreateSound()
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