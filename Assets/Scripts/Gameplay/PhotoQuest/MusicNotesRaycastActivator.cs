using UnityEngine;

public class MusicNotesRaycastActivator : MonoBehaviour
{
    //[SerializeField] GameCondition gameCondition;
    [SerializeField] public Events monologueToTrigger;
    [SerializeField] public int monologueDelay;
    [SerializeField] ItemDissolve item;
    [SerializeField] int myOrderIndex;

    private void Start()
    {
        //if (GameManager.Instance.GetCondition(GameCondition.MusicSafeDoorOpen))
        //{
        //    SetChildrenActive(true);
        //}
        //else
        //{
        //    SetChildrenActive(false);
        //}
    }
    public bool SetChildrenActive(bool active)
    {
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
