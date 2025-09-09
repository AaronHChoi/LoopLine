using Assets;
using UnityEngine;

public class ActiveBox : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IActivable>(out IActivable activable))
        {
            activable.Active();
            gameObject.SetActive(false);
        }
    }
}
