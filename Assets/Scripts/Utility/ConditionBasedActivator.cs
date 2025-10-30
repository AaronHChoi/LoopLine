using UnityEngine;

public class ConditionBasedActivator : MonoBehaviour
{
    [SerializeField] GameCondition conditionCheck;
    [SerializeField] bool active;

    void Start()
    {
        bool conditionValue = GameManager.Instance.GetCondition(conditionCheck);

        if (conditionValue)
        {
            gameObject.SetActive(active);
        }
    }
}