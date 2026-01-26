using UnityEngine;
using Core.Audio;
using Core.DependencyInjection;

public class BGMController : MonoBehaviour
{
    [SerializeField] SoundData bgmData;
    [SerializeField] SoundData noiseData;

    ISoundManager soundManager;

    private void Awake()
    {
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();
    }
    private void Start()
    {
        soundManager.CreateSound()
            .WithSoundData(bgmData)
            .Play();

        soundManager.CreateSound()
            .WithSoundData(noiseData)
            .Play();
    }
}