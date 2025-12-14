using UnityEngine;

public class RaycastActivator : MonoBehaviour
{
    [SerializeField] GameCondition gameCondition;
    [SerializeField] public Events monologueToTrigger;
    [SerializeField] public int monologueDelay;
    [SerializeField] ItemDissolve item;
    [SerializeField] int myOrderIndex;

    private void Awake()
    {
        SetChildrenActive(false);
    }
    public bool SetChildrenActive(bool active)
    {
        bool isRightOrder = false;
        if (active)
        {
            if (myOrderIndex != GameManager.Instance.currentPhotoIndex)
            {
                return isRightOrder;
            }
            else
            {
                isRightOrder = true;
            }
            GameManager.Instance.currentPhotoIndex++;
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }

        if (item != null & active)
        {
            item.TakePhoto();
        }

        return true;
    }
}