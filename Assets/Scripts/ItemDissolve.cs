using System.Collections.Generic;
using UnityEngine;

public class ItemDissolve : MonoBehaviour
{
    [SerializeField] List<DissolveControllerScript> item;
    [SerializeField] GameCondition clueGameCondition;
    [SerializeField] float delay;

    public void DeactivatePhoto()
    {
        GameManager.Instance.SetCondition(clueGameCondition, true);

        foreach (var c in item)
        {
            c.ActivateDissolve();
        }

    }
    public void TakePhoto()
    {
        DelayUtility.Instance.Delay(delay, DeactivatePhoto);
    }
}