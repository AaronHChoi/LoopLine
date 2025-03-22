using Assets;
using UnityEngine;

public class FireController : MonoBehaviour, IActivable
{
    public GameObject Fire;
    public CarriageController Carriage;
    bool IActivable.Active()
    {
        Fire.SetActive(!Fire.activeInHierarchy);
        Carriage.MoveCarriage(Fire.activeInHierarchy);
        return true;
    }
}
