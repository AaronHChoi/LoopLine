using DependencyInjection;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSoundListenerMindPlace : MonoBehaviour
{
    [Header("UI Sounds")]
    [SerializeField] SoundData inventoryOpenSound;
    [SerializeField] SoundData inventoryCloseSound;

    [SerializeField] SoundData finalDoorSound;

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

    void OnFinalDoor(FinalQuestCompleteEvent ev) 
    {
        SoundManager.Instance.CreateSound()
            .WithSoundData(finalDoorSound)
            .Play();
    }

}