using DependencyInjection;
using UnityEngine;

public class TutorialInteract : ItemInteract
{
    protected override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
    }
    public override bool Interact()
    {
        EventBus.Publish(new PlayerGrabItemEvent());
        return base.Interact();
    }
}
