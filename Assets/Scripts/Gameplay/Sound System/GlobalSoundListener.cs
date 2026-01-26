using System.Collections.Generic;
using UnityEngine;
using Core.EventBus;
using Core.Utilities;
using Core.Audio;
using Core.DependencyInjection;

public class GlobalSoundListener : Singleton<GlobalSoundListener>
{
    [Header("Player")]
    [SerializeField] private List<SoundData> PlayerSteps;
    [SerializeField] SoundData transition;

    ISoundManager soundManager;

    protected override void Awake()
    {
        base.Awake();

        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();
    }
    private void OnEnable()
    {
        EventBus.Subscribe<PlayerStepEvent>(PlayPlayerStepSound);
        EventBus.Subscribe<TransitionEvent>(PlayTransition);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerStepEvent>(PlayPlayerStepSound);
        EventBus.Unsubscribe<TransitionEvent>(PlayTransition);
    }
    void PlayPlayerStepSound(PlayerStepEvent st)
    {
        if (PlayerSteps.Count == 0) return;
        int randomIndex = Random.Range(0, PlayerSteps.Count);
        SoundData stepSound = PlayerSteps[randomIndex];
        soundManager.CreateSound()
            .WithSoundData(stepSound)
            .WithRandomPitch()
            .Play();
    }
    void PlayTransition(TransitionEvent ev)
    {
        soundManager.CreateSound()
           .WithSoundData(transition)
           .WithRandomPitch()
           .Play();
    }
}