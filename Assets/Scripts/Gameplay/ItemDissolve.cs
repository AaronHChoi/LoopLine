using System.Collections.Generic;
using UnityEngine;
using Core.Utilities;

public class ItemDissolve : MonoBehaviour
{
    [SerializeField] List<DissolveControllerScript> item;
    [SerializeField] float delay;

    public void DeactivatePhoto()
    {
        if (this == null)
        {
            return;
        }

        if (item != null)
        {
            foreach (var c in item)
            {
                if (c != null)
                {
                    c.ActivateDissolve();
                }
            }
        }
    }
    public void TakePhoto()
    {
        if (this == null || !gameObject.activeInHierarchy)
        {
            return;
        }

        DelayUtility.Instance.Delay(delay, DeactivatePhoto);
    }
}