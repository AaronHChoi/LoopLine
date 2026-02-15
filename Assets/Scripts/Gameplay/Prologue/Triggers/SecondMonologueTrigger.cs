using UnityEngine;
using Core.DependencyInjection;
using Core.Utilities;
using Core.Data;
using System.Collections;

public class SecondMonologueTrigger : BaseTrigger
{
    private Coroutine timerCoroutine;
    private IMonologueSpeaker monologueSpeaker;

    private void Awake()
    {
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
    }
    private void Start()
    {
        if (GameManager.Instance.TrainLoop != 0)
        {
            this.gameObject.SetActive(false);
        }
        else if (!GameManager.Instance.GetCondition(GameCondition.StayMonologueQueued))
        {
            GameManager.Instance.SetCondition(GameCondition.StayMonologueQueued, true);

            timerCoroutine = StartCoroutine(MonologueTimerRoutine());
        }
    }
    private IEnumerator MonologueTimerRoutine()
    {
        yield return new WaitForSeconds(15f);

        monologueSpeaker.StartMonologue(Events.SecondMonologueStay);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}