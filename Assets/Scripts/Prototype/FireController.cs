using Assets;
using UnityEngine;

public class FireController : MonoBehaviour, IActivable
{
    public GameObject Fire;
    public GameObject FirePlayer;
    public CarriageController Carriage;
    bool IActivable.Active()
    {
        Fire.SetActive(!Fire.activeInHierarchy);
        FirePlayer.SetActive(!FirePlayer.activeInHierarchy);
        Carriage.MoveCarriage(Fire.activeInHierarchy);
        return true;
    }
}
