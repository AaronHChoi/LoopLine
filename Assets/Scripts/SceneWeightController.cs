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
        if (!isActive) return;

        foreach (var rule in rules)
        {
            if (rule.conditionToCheck == condition)
            {
                ApplyRule(rule);
            }
        }
    }
    void ApplyRule(WeightRule rule)
    {
        foreach (var target in rule.scenesToModify)
        {
            Debug.Log($"[Rule: {rule.ruleName}] Condition {rule.conditionToCheck} is TRUE. Updating {target.sceneName} to {target.weightIfConditionTrue}");

            gameSceneManager.ChangeSceneWeighth(target.sceneName, target.weightIfConditionTrue);
        }
    }
    void ApplyAllRules()
    {
        foreach (var rule in rules)
        {
            bool currentState = GameManager.Instance.GetCondition(rule.conditionToCheck);
            if (currentState)
            {
                ApplyRule(rule);
            }
        }
    }
}
public interface ISceneWeightController
{
    void HandleConditionChanged(GameCondition condition, bool isActive);
}