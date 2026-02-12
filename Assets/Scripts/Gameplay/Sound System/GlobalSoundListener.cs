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

    [Header("Interaction Sounds")]
    [SerializeField] List<SoundData> grabSounds;

    ISoundManager soundManager;

    protected override void Awake()
    {
        base.Awake();

        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();
    }
    private void OnEnable()
    {
        EventBus.Subscribe<PlayerStepEvent>(PlayPlayerStepSound);
        EventBus.Subscribe<PlayerGrabItemEvent>(OnGrabItems);
        EventBus.Subscribe<TransitionEvent>(PlayTransition);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerStepEvent>(PlayPlayerStepSound);
        EventBus.Unsubscribe<PlayerGrabItemEvent>(OnGrabItems);
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
    void OnGrabItems(PlayerGrabItemEvent ev)
    {
        soundManager.CreateSound()
            .WithSoundData(grabSounds[Random.Range(0, grabSounds.Count)])
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