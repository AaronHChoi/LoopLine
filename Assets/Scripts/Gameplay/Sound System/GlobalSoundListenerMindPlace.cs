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

    IInventoryUI inventoryUI;
    ISoundManager soundManager;

    private void Awake()
    {
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();
        inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
    }
    private void OnEnable()
    {
        Debug.Log($"[GlobalSoundListener] Intentando suscribir a PlayerInventoryEvent en el objeto: {gameObject.name}");
        EventBus.Subscribe<PlayerInventoryEvent>(OnInventoryToggled);
        EventBus.Subscribe<FinalQuestCompleteEvent>(OnFinalDoor);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerInventoryEvent>(OnInventoryToggled);
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
    void OnFinalDoor(FinalQuestCompleteEvent ev) 
    {
        soundManager.CreateSound()
            .WithSoundData(finalDoorSound)
            .Play();
    }
}