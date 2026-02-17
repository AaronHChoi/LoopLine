using UnityEngine;

namespace Gameplay.QuestPhoto
{
	public class MonologueTrigger : MonoBehaviour, IMonologueTrigger
	{
		[field: SerializeField] public Events monologueToTrigger { get; set; }
        [field: SerializeField] public int monologueDelay { get; set; }
	}
}