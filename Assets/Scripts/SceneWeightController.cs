using System.Collections.Generic;
using UnityEngine;

public class SceneWeightController : MonoBehaviour
{
    [System.Serializable]
    public struct WeightRule
    {
        public string name;
        public GameCondition conditionToCheck;
        public string sceneToModify;
        public int weightIfConditionTrue;
        public int weightIfConditionFalse;
    }

    [Header("Rules Configuration")]
    [SerializeField] List<WeightRule> rules = new List<WeightRule>();
}