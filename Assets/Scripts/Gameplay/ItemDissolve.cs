using System.Collections.Generic;
using UnityEngine;
using Core.Utilities;

public class ItemDissolve : MonoBehaviour
{
    [SerializeField] List<DissolveControllerScript> item;
    [SerializeField] float delay;

    public void DeactivatePhoto()
    {
        if (item != null)
        {
            foreach (var c in item)
            {
                c.ActivateDissolve();
            }
        }
    }
    public void TakePhoto()
    {
        DelayUtility.Instance.Delay(delay, DeactivatePhoto);
    }
}