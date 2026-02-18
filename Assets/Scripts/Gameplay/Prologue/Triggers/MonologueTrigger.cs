using Core.DependencyInjection;
using Core.UI;
using Core.Utilities;
using UnityEngine;

namespace Gameplay.Prologue
{
    public class MonologueTrigger : BaseTrigger
    {
        IMonologueSpeaker monologueSpeaker;

        private void Awake()
        {
            monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>(MonologueID.Player);
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
}