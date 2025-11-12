using UnityEngine;

public class RaycastActivator : MonoBehaviour
{
    [SerializeField] GameCondition gameCondition;
    bool afterAwake = false;
    private void Awake()
    {
        SetChildrenActive(false);
        afterAwake = true;
    }
    public void SetChildrenActive(bool active)
    {
        if (afterAwake)
        {
            GameManager.Instance.SetCondition(gameCondition, true);
        }
        
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(active);
        }
    }
}