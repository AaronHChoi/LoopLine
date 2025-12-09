using UnityEngine;

public class GlobalSoundListener : MonoBehaviour
{
    [Header("UI Sounds")]
    [SerializeField] SoundData inventoryOpenSound;
    [SerializeField] SoundData inventoryCloseSound;

    private void OnEnable()
    {
        EventBus.Subscribe<PlayerInventoryEvent>(OnInventoryToggled);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerInventoryEvent>(OnInventoryToggled);
    }
    void OnInventoryToggled(PlayerInventoryEvent ev)
    {
        SoundData soundToPlay = ev.IsOpening ? inventoryOpenSound : inventoryCloseSound;

        if (soundToPlay != null)
        {
            SoundManager.Instance.CreateSound()
                .WithSoundData(soundToPlay)
                .WithRandomPitch()
                .Play();
        }
    }
}