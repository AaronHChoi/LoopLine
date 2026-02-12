using UnityEngine;
using Core.EventBus;
using Core.DependencyInjection;
using Core.Audio;

public class GlobalSoundListenerTrain : MonoBehaviour
{
    [SerializeField] SoundData pushClockButton;

    ISoundManager soundManager;

    private void Awake()
    {
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();    
    }
    private void OnEnable()
    {
        EventBus.Subscribe<ClockSyncEvent>(OnPushClockButton);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<ClockSyncEvent>(OnPushClockButton);
    }
    void OnPushClockButton(ClockSyncEvent ev)
    {
        Vector3 buttonPosition = new Vector3(-1.898f, 2.51999998f, -20.7919998f);

        soundManager.CreateSound()
                .WithSoundData(pushClockButton)
                .WithSoundPosition(buttonPosition)
                .Play();
    }
}