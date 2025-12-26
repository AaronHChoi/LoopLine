using UnityEngine;

public class RaycastActivator : MonoBehaviour
{
    [SerializeField] public Events monologueToTrigger;
    [SerializeField] public int monologueDelay;
    [SerializeField] ItemDissolve item;
    [SerializeField] int myOrderIndex;
    [SerializeField] bool musicalNote;
    [SerializeField] string idMusicalNote;
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
            if (musicalNote)
            {
                if (isFromPhoto && GameManager.Instance.GetCondition(GameCondition.MusicSafeDoorOpen))
                {
                    item.TakePhoto();
                    if (GameManager.Instance.GetCondition(GameCondition.MusicNote1) &&
                        GameManager.Instance.GetCondition(GameCondition.MusicNote2) && 
                        GameManager.Instance.GetCondition(GameCondition.MusicNote3) &&
                        GameManager.Instance.GetCondition(GameCondition.MusicNote4))
                    {
                        GameManager.Instance.SetCondition(GameCondition.AllMusicNotesCollected, true);
                    }
                    DelayUtility.Instance.Delay(2f, () => gameObject.SetActive(false));
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