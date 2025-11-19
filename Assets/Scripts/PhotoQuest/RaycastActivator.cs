using System.Collections.Generic;
using UnityEngine;

public class RaycastActivator : MonoBehaviour
{
    [SerializeField] GameCondition gameCondition;
    [SerializeField] public Events monologueToTrigger;
    [SerializeField] ItemDissolve item;
    [SerializeField] int myOrderIndex;

    private void Awake()
    {
        SetChildrenActive(false);
    }
    public void SetChildrenActive(bool active)
    {
        if (active)
        {
            if (myOrderIndex != GameManager.Instance.currentPhotoIndex)
            {
                return;
            }
            GameManager.Instance.currentPhotoIndex++;
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }

        if (item != null && active)
        {
            item.TakePhoto();
        }
    }
}