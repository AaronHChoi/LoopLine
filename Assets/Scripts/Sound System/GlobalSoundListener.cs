using System.Collections.Generic;
using UnityEngine;

public class GlobalSoundListener : Singleton<GlobalSoundListener>
{
    [Header("Player")]
    [SerializeField] private List<SoundData> PlayerSteps;

    private void OnEnable()
    {
        EventBus.Subscribe<PlayerStepEvent>(PlayPlayerStepSound);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerStepEvent>(PlayPlayerStepSound);
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
}