using UnityEngine;
using Core.Data;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] triggersToDisable;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.SetCondition(GameCondition.PrologueDoorsLocked, true);

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