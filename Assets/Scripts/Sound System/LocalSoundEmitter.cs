using SoundSystem;
using UnityEngine;

public class LocalSoundEmitter : MonoBehaviour
{
    [SerializeField] EventsID emitterID;
    [SerializeField] SoundData sound;
    [SerializeField] bool PlayOnStart = false;
    SoundEmitter activeEmitter;

    private void OnEnable()
    {
        EventBus.Subscribe<UnlockDoorEvent>(OnToggleEvent);

        if (PlayOnStart)
        {
            ToggleSound(true);
        }
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<UnlockDoorEvent>(OnToggleEvent);

        ToggleSound(false);
    }
    void OnToggleEvent(UnlockDoorEvent ev)
    {
        if (ev.SoundID == emitterID || ev.SoundID == EventsID.All)
        {
            ToggleSound(ev.ShouldPlay);
        }
    }
    private void ToggleSound(bool shouldPlay)
    {
        if (shouldPlay)
        {
            if (activeEmitter == null)
            {
                activeEmitter = SoundManager.Instance.CreateSound()
                   .WithSoundData(sound)
                   .WithSoundPosition(transform.position)
                   .Play();

                if (activeEmitter != null)
                {
                    activeEmitter.transform.SetParent(this.transform);
                }
            }
        }
    }
}