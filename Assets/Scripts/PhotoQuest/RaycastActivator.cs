using UnityEngine;

public class RaycastActivator : MonoBehaviour
{
    [SerializeField] public Events monologueToTrigger;
    [SerializeField] public int monologueDelay;
    [SerializeField] ItemDissolve item;
    [SerializeField] int myOrderIndex;
    [SerializeField] bool musicalNote;
    [SerializeField] string idMusicalNote;
    [SerializeField] bool LOOP4;
    private void Awake()
    {
        if (GameManager.Instance.GetCondition(GameCondition.MusicSafeDoorOpen) && musicalNote)
        {
            SetChildrenActive(true, false);
        }
        else
        {
            SetChildrenActive(false, false);
        }
    }
    public bool SetChildrenActive(bool active, bool isFromPhoto = false)
    {
        //bool isRightOrder = false;
        //if (active)
        //{
        //    if (myOrderIndex != GameManager.Instance.currentPhotoIndex)
        //    {
        //        return isRightOrder;
        //    }
        //    else
        //    {
        //        isRightOrder = true;
        //    }
        //    GameManager.Instance.currentPhotoIndex++;
        //}

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }

        if (item != null & active)
        {
            if (musicalNote)
            {
                if (isFromPhoto && GameManager.Instance.GetCondition(GameCondition.MusicSafeDoorOpen))
                {
                    item.TakePhoto();

                    if (LOOP4 && !GameManager.Instance.GetCondition(GameCondition.FirstTimeLoop4))
                    {
                        GameManager.Instance.SetCondition(GameCondition.LOOP4, true);
                        GameManager.Instance.SetCondition(GameCondition.FirstTimeLoop4, true);
                        GameManager.Instance.SetCondition(GameCondition.TeleportAvailable, false);
                    }
                }
            }
            else
            {
                item.TakePhoto();
            }
        }

        return true;
    }
}