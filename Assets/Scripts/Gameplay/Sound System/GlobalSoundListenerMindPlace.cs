using System.Collections.Generic;
using UnityEngine;
using Core.EventBus;
using Core.DependencyInjection;
using Core.Audio;

public class GlobalSoundListenerMindPlace : MonoBehaviour
{
    [Header("UI Sounds")]
    [SerializeField] SoundData inventoryOpenSound;
    [SerializeField] SoundData inventoryCloseSound;

    [SerializeField] SoundData finalDoorSound;

    [Header("Interaction Sounds")]
    [SerializeField] List<SoundData> grabSounds;

    IInventoryUI inventoryUI;
    ISoundManager soundManager;

    private void Awake()
    {
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();
        inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
    }
    private void OnEnable()
    {
        EventBus.Subscribe<PlayerInventoryEvent>(OnInventoryToggled);
        EventBus.Subscribe<PlayerGrabItemEvent>(OnGrabItems);
        EventBus.Subscribe<FinalQuestCompleteEvent>(OnFinalDoor);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerInventoryEvent>(OnInventoryToggled);
        EventBus.Unsubscribe<PlayerGrabItemEvent>(OnGrabItems);
        EventBus.Unsubscribe<FinalQuestCompleteEvent>(OnFinalDoor);
    }
    void OnInventoryToggled(PlayerInventoryEvent ev)
    {
        SoundData soundToPlay = ev.IsOpening ? inventoryOpenSound : inventoryCloseSound;

        if (soundToPlay != null && !inventoryUI.isFirstTimeOpening)
        {
            soundManager.CreateSound()
                .WithSoundData(soundToPlay)
                .WithRandomPitch()
                .Play();
        }
    }
    void OnGrabItems(PlayerGrabItemEvent ev)
    {
        soundManager.CreateSound()
            .WithSoundData(grabSounds[Random.Range(0, grabSounds.Count)])
            .Play();
    }
    void OnFinalDoor(FinalQuestCompleteEvent ev) 
    {
        soundManager.CreateSound()
            .WithSoundData(finalDoorSound)
            .Play();
    }
}