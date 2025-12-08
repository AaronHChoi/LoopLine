using SoundSystem;
using UnityEngine;

public class LocalSoundEmitter : MonoBehaviour
{
    [SerializeField] SoundData loopSound;
    SoundEmitter activeEmitter;

    private void Start()
    {
        activeEmitter = SoundManager.Instance.CreateSound()
            .WithSoundData(loopSound)
            .WithSoundPosition(transform.position)
            .Play();

        activeEmitter.transform.SetParent(this.transform);
    }
}