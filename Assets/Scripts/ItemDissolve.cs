using System.Collections.Generic;
using UnityEngine;

public class ItemDissolve : MonoBehaviour
{
    [SerializeField] List<DissolveControllerScript> item;
    [SerializeField] GameCondition clueGameCondition;
    [SerializeField] float delay;

    public void DeactivatePhoto()
    {


        foreach (var c in item)
        {
            c.ActivateDissolve();
        }

        GameManager.Instance.SetCondition(clueGameCondition, true);
    }
    public void TakePhoto()
    {
        DelayUtility.Instance.Delay(delay, DeactivatePhoto);
    }
}