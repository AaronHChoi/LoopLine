using UnityEngine;

public class ClockButton : MonoBehaviour, IInteract
{
    Animator buttonAnimator;

    [SerializeField] SoundData sound;

    [SerializeField] private float cooldownDuration = 1f;
    private float nextInteractTime = 0f;

    private void Awake()
    {
        buttonAnimator = GetComponent<Animator>();
    }
    public string GetInteractText()
    {
        throw new System.NotImplementedException();
    }
    public void Interact()
    {
        if (Time.time < nextInteractTime)
        {
            return;
        }
        nextInteractTime = Time.time + cooldownDuration;

        SoundManager.Instance.CreateSound()
                .WithSoundData(sound)
                .WithSoundPosition(transform.position)
                .Play();

        buttonAnimator.SetTrigger("Interact");

        EventBus.Publish(new ClockSyncEvent());
    }
}