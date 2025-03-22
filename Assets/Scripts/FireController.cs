using Assets;
using UnityEngine;

public class FireController : MonoBehaviour, IActivable
{
    public GameObject Fire;
    public CarriageController Carriage;
    bool IActivable.Active()
    {
        Fire.SetActive(!Fire.activeInHierarchy);
        Carriage.Move = Fire.activeInHierarchy;
        return true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
