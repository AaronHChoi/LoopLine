using UnityEngine;

public class ConditionTrigger : MonoBehaviour
{
    [SerializeField] GameCondition condition;
    [SerializeField] bool targetValue = true;

    public void TriggerCondition()
    {
        GameManager.Instance.SetCondition(condition, targetValue);
    }
}