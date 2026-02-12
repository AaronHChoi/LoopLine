using UnityEngine;
using Core.EventBus;
using Core.Utilities;

public class ClockButton : MonoBehaviour, IInteract
{
    Animator buttonAnimator;

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
        buttonAnimator.SetTrigger("Interact");

        if (Time.time < nextInteractTime)
        {
            return;
        }
        nextInteractTime = Time.time + cooldownDuration;

        DelayUtility.Instance.Delay(0.5f, () => EventBus.Publish(new ClockSyncEvent()));
    }
}