using Core.Audio;
using UnityEngine;

public class AudioTape : MonoBehaviour, IAudioTape
{
    [Header("Audio")]
    [SerializeField] SoundData soundData;

    [SerializeField] public Events monologueToTrigger { get; }

    public SoundData GetSoundData()
    {
        return soundData;
    }
}
public interface IAudioTape
{
    Events monologueToTrigger { get; }
    SoundData GetSoundData();
}


