using DependencyInjection;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSoundListener : MonoBehaviour
{
    [Header("UI Sounds")]
    [SerializeField] SoundData inventoryOpenSound;
    [SerializeField] SoundData inventoryCloseSound;
    
    [Header("Player")]
    [SerializeField] private List<SoundData> PlayerSteps;

    [Header("Interaction Sounds")]
    [SerializeField] List<SoundData> grabSounds;

    IInventoryUI inventoryUI;
    private void Awake()
    {
        inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
    }
    private void OnEnable()
    {
        EventBus.Subscribe<PlayerInventoryEvent>(OnInventoryToggled);
        EventBus.Subscribe<PlayerGrabItemEvent>(OnGrabItems);
        EventBus.Subscribe<PlayerStepEvent>(PlayPlayerStepSound);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerInventoryEvent>(OnInventoryToggled);
        EventBus.Unsubscribe<PlayerGrabItemEvent>(OnGrabItems);
        EventBus.Unsubscribe<PlayerStepEvent>(PlayPlayerStepSound);
    }
    void OnInventoryToggled(PlayerInventoryEvent ev)
    {
        SoundData soundToPlay = ev.IsOpening ? inventoryOpenSound : inventoryCloseSound;

        if (soundToPlay != null && !inventoryUI.isFirstTimeOpening)
        {
            SoundManager.Instance.CreateSound()
                .WithSoundData(soundToPlay)
                .WithRandomPitch()
                .Play();
        }
    }
    void OnGrabItems(PlayerGrabItemEvent ev)
    {
        SoundManager.Instance.CreateSound()
            .WithSoundData(grabSounds[Random.Range(0, grabSounds.Count)])
            .Play();
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