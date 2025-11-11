using UnityEngine;

public class RaycastActivator : MonoBehaviour
{
    private void Awake()
    {
        SetChildrenActive(false);
    }
    public void SetChildrenActive(bool active)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
    }
}