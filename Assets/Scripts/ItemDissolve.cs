using System.Collections.Generic;
using UnityEngine;

public class ItemDissolve : MonoBehaviour
{
    [SerializeField] List<DissolveControllerScript> item;
    [SerializeField] GameCondition clueGameCondition;
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
        Debug.Log($"[ItemDissolve] Enviando condición: {clueGameCondition} (ID: {(int)clueGameCondition})");
        GameManager.Instance.SetCondition(clueGameCondition, true);
        DelayUtility.Instance.Delay(delay, DeactivatePhoto);
    }
}