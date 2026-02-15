using UnityEngine;

namespace Core.Utilities
{
	public abstract class BaseTrigger : MonoBehaviour
	{
		[Header("Base Trigger Settings")]
		[SerializeField] protected GameObject[] triggersToDisable;

        protected virtual void OnTriggerEnter(Collider other)
        {
            DisableAllTriggers();
        }
        private void DisableAllTriggers()
        {
            foreach (GameObject obj in triggersToDisable)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
    } 
}