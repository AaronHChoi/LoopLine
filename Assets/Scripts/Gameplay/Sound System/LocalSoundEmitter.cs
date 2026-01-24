using UnityEngine;

public class LocalSoundEmitter : MonoBehaviour
{
    [SerializeField] EventsID emitterID;
    [SerializeField] SoundData sound;
    [SerializeField] bool PlayOnStart = false;
     
    private void OnEnable()
    {
        EventBus.Subscribe<DoorEvent>(OnToggleEvent);

        if (PlayOnStart)
        {
            ToggleSound(true);
        }
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<DoorEvent>(OnToggleEvent);

        ToggleSound(false);
    }
    void OnToggleEvent(DoorEvent ev)
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
            SoundManager.Instance.CreateSound()
                .WithSoundData(sound)
                .WithSoundPosition(transform.position)
                .Play();
        }
    }
}