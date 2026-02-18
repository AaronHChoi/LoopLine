using UnityEngine;
using Core.DependencyInjection;

namespace Gameplay.QuestPhoto
{
	public class MonologueTrigger : MonoBehaviour
	{
        public Events monologueToTrigger;
        public int monologueDelay;

        IMonologueSpeaker monologueSpeaker;

        private void Awake()
        {
            monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();    
        }
        public void StartMonologueAfterPhoto()
        {
            monologueSpeaker.StartMonologue(monologueToTrigger);
        }
    }
}