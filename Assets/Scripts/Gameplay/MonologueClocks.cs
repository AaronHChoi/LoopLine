using Core.DependencyInjection;
using Core.UI;
using Core.Utilities;
using UnityEngine;

public class MonologueClocks : BaseTrigger
{
    private IMonologueSpeaker monologueSpeaker;

    private void Awake()
    {
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>(MonologueID.Player);
    }
    private void Start()
    {
        if (GameManager.Instance.TrainLoop != 2)
        {
            this.gameObject.SetActive(false);
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        monologueSpeaker.StartMonologue(Events.MonologueFirstClock);

        base.OnTriggerEnter(other);
    }
}