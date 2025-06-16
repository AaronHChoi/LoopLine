using UnityEngine;

public class BGMController : MonoBehaviour
{
    [SerializeField] SoundData bgmData;
    [SerializeField] SoundData noiseData;
    private void Start()
    {
        SoundManager.Instance.CreateSound()
            .WithSoundData(bgmData)
            .Play();

        SoundManager.Instance.CreateSound()
            .WithSoundData(noiseData)
            .Play();
    }
}
