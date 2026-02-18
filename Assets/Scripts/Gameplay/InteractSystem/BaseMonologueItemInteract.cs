using Core.DependencyInjection;
using Core.UI;
using UnityEngine;

public class BaseMonologueItemInteract : BaseItemInteract
{
    [SerializeField] private Events Event;

    private IMonologueSpeaker monologueSpeaker;

    protected override void Awake()
    {
        base.Awake();
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>(MonologueID.Player);
    }
    public override void Start()
    {
        base.Start();
    }
    public override bool Interact()
    {
        if (Event != Events.None)
        {
            monologueSpeaker.StartMonologue(Event);
        }
        return base.Interact();
    }
}
