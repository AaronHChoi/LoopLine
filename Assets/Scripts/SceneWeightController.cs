using DependencyInjection;
using System.Collections.Generic;
using UnityEngine;

public class SceneWeightController : MonoBehaviour, ISceneWeightController
{
    [System.Serializable]
    public struct SceneWeightTarget
    {
        public string sceneName;
        public int weightIfConditionTrue;
        public int weightIfConditionFalse;
    }

    [System.Serializable]
    public struct WeightRule
    {
        public string ruleName;
        public GameCondition conditionToCheck;
        public List<SceneWeightTarget> scenesToModify;
    }

    [Header("Rules Configuration")]
    [SerializeField] List<WeightRule> rules = new List<WeightRule>();

    IGameSceneManager gameSceneManager;

    private void Awake()
    {
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
    }
    private void Start()
    {
        if (GameManager.Instance.TrainLoop >= 1)
        {
            ApplyAllRules();
        }
    }
    public void HandleConditionChanged(GameCondition condition, bool isActive)
    {
        foreach (var rule in rules)
        {
            if (rule.conditionToCheck == condition)
            {
                ApplyRule(rule, isActive);
            }
        }
    }
    void ApplyRule(WeightRule rule, bool conditionState)
    {
        foreach (var target in rule.scenesToModify)
        {
            int newWeight = conditionState ? target.weightIfConditionTrue : target.weightIfConditionFalse;

            Debug.Log($"[Rule: {rule.ruleName}] Condition {rule.conditionToCheck} is {conditionState}. Updating {target.sceneName} to {newWeight}");

            gameSceneManager.ChangeSceneWeighth(target.sceneName, newWeight);
        }
    }
    void ApplyAllRules()
    {
        foreach (var rule in rules)
        {
            bool currentState = GameManager.Instance.GetCondition(rule.conditionToCheck);
            ApplyRule(rule, currentState);
        }
    }
}
public interface ISceneWeightController
{
    void HandleConditionChanged(GameCondition condition, bool isActive);
}