using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ItemDissolve : MonoBehaviour
{
    [SerializeField] List<DissolveControllerScript> item;
    [SerializeField] GameCondition clueGameCondition;
    [SerializeField] float delay;

    [SerializeField] bool LOOP4;
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

        if (LOOP4 && !GameManager.Instance.GetCondition(GameCondition.FirstTimeLoop4))
        {
            GameManager.Instance.SetCondition(GameCondition.LOOP4, true);
            GameManager.Instance.SetCondition(GameCondition.FirstTimeLoop4, true);
            GameManager.Instance.SetCondition(GameCondition.TeleportAvailable, false);
        }
    }
}