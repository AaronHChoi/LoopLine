using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    private void Start()
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
    public void DeactivateSelfWithDelay(float delay)
    {
        DelayUtility.Instance.Delay(delay, () => gameObject.SetActive(false));
    }
}