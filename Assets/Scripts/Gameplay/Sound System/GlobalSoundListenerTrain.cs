using UnityEngine;
using Core.EventBus;
using Core.DependencyInjection;
using Core.Audio;

public class GlobalSoundListenerTrain : MonoBehaviour
{
    [SerializeField] SoundData pushClockButton;
    [SerializeField] SoundData brokenGlass;

    [SerializeField] GameObject brokenGlassPositon;

    ISoundManager soundManager;

    private void Awake()
    {
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();    
    }
    private void OnEnable()
    {
        EventBus.Subscribe<ClockSyncEvent>(OnPushClockButton);
        EventBus.Subscribe<GlassBreakingSound>(OnGlassBreaking);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<ClockSyncEvent>(OnPushClockButton);
        EventBus.Unsubscribe<GlassBreakingSound>(OnGlassBreaking);
    }
    void OnPushClockButton(ClockSyncEvent ev)
    {
        Vector3 buttonPosition = new Vector3(-1.898f, 2.51999998f, -20.7919998f);

        soundManager.CreateSound()
                .WithSoundData(pushClockButton)
                .WithSoundPosition(buttonPosition)
                .Play();
    }
    void OnGlassBreaking(GlassBreakingSound ev)
    {
        soundManager.CreateSound()
            .WithSoundData(brokenGlass)
            .WithSoundPosition(brokenGlassPositon.transform.position)
            .Play();
    }
}