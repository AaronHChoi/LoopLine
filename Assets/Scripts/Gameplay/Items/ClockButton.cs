using UnityEngine;
using Core.EventBus;
using Core.Audio;
using Core.DependencyInjection;

public class ClockButton : MonoBehaviour, IInteract
{
    Animator buttonAnimator;

    [SerializeField] SoundData sound;

    [SerializeField] private float cooldownDuration = 1f;
    private float nextInteractTime = 0f;

    ISoundManager soundManager;

    private void Awake()
    {
        buttonAnimator = GetComponent<Animator>();
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();
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

        soundManager.CreateSound()
                .WithSoundData(sound)
                .WithSoundPosition(transform.position)
                .Play();

        buttonAnimator.SetTrigger("Interact");

        EventBus.Publish(new ClockSyncEvent());
    }
}