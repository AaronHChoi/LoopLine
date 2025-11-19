using System.Collections.Generic;
using UnityEngine;

public class ConditionListActivator : MonoBehaviour
{
    [SerializeField] List<ConditionCheckModel> conditionsCheck;
    [SerializeField] bool active;

    void Start()
    {
        bool caseNotFound = false;
        foreach (var condition in conditionsCheck)
        {
            if (GameManager.Instance.GetCondition(condition.gameCondition) != condition.isTrue)
            {
                caseNotFound = true;
            }
        }

        if (caseNotFound)
        {
            gameObject.SetActive(!active);
        }
        else
        {
            gameObject.SetActive(active);
        }
    }
}
[System.Serializable]
public struct ConditionCheckModel
{
    public GameCondition gameCondition;
    public bool isTrue;
}