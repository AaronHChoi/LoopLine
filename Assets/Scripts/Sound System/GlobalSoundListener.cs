using System.Collections.Generic;
using UnityEngine;

public class GlobalSoundListener : Singleton<GlobalSoundListener>
{
    [Header("Player")]
    [SerializeField] private List<SoundData> PlayerSteps;
    [SerializeField] SoundData transition;

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
        SoundManager.Instance.CreateSound()
            .WithSoundData(stepSound)
            .WithRandomPitch()
            .Play();
    }
    void PlayTransition(TransitionEvent ev)
    {
        SoundManager.Instance.CreateSound()
           .WithSoundData(transition)
           .WithRandomPitch()
           .Play();
    }
}