using UnityEngine;
using Core.Utilities;
using Core.DependencyInjection;

public class MonologueTrigger : BaseTrigger
{
    IMonologueSpeaker monologueSpeaker;

    private void Awake()
    {
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
    }
    private void Start()
    {
        if (GameManager.Instance.TrainLoop != 1)
        {
            this.gameObject.SetActive(false);
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        monologueSpeaker.StartMonologue(Events.LOOP2_Monologue);

        base.OnTriggerEnter(other);
    }
}