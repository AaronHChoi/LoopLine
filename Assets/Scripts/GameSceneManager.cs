using System.Collections;
using System.Collections.Generic;
using DependencyInjection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : Singleton<GameSceneManager>, IGameSceneManager
{
    [Header("Scene Settings")]
    [SerializeField] private WeightScene firstScene;
    [SerializeField] private List<WeightScene> weightedScenes = new List<WeightScene>();

    [Header("Active Scenes")]
    [SerializeField] private List<string> activeScenes = new List<string>();

    bool isInInitialLoop = true;

    ITeleportLoop teleportLoop;

    protected override void Awake()
    {
        base.Awake();

        teleportLoop = InterfaceDependencyInjector.Instance.Resolve<ITeleportLoop>();
    }
    private void Start()
    {
        if (IsCurrentScene("04. Train"))
        {
            StartCoroutine(LoadSceneAsync(firstScene.sceneName));
        }
    }
    private void OnEnable()
    {
        if (teleportLoop != null)
        {
            teleportLoop.OnTeleportTrain += CheckTrainLoop;
        }
    }
    private void OnDisable()
    {
        if (teleportLoop != null)
        {
            teleportLoop.OnTeleportTrain -= CheckTrainLoop;
        }
    }
    public void CheckTrainLoop()
    {
        if (GameManager.Instance.TrainLoop >= 2)
        {
            GameManager.Instance.SetCondition(GameCondition.TeleportAvailable, true);
            SetInitialLoop(false);
        }
    }
    public void SetInitialLoop(bool isActive)
    {
        isInInitialLoop = isActive;
    }
    public bool GetIsInInitialLoop()
    {
        return isInInitialLoop;
    }
    public void LoadRandomScene()
    {
        string selectedScene = GetRandomSceneByWeight();
        StartCoroutine(LoadSceneAsync(selectedScene));
    }
    public void LoadNextScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }
    public bool IsCurrentScene(string _sceneName)
    {
        return SceneManager.GetActiveScene().name == _sceneName;
    }
    private string GetRandomSceneByWeight()
    {
        float totalWeight = 0f;
        foreach (var scene in weightedScenes)
            totalWeight += scene.weight;

        float randomValue = Random.value * totalWeight;
        float current = 0f;

        foreach (var scene in weightedScenes)
        {
            current += scene.weight;
            if (randomValue <= current)
                return scene.sceneName;
        }
        return weightedScenes[0].sceneName;
    }
    public IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
            yield return null;

        activeScenes.Add(sceneName);
    }
    public void UnloadLastScene()
    {
        if (activeScenes.Count == 0)
        {           
            return;
        }
        string lastScene = activeScenes[activeScenes.Count - 1];
        StartCoroutine(UnloadSceneAsync(lastScene));
    }

    public void ChangeSceneWeighth(string SceneName, int newWeight)
    {
        for (int i = 0; i < weightedScenes.Count; i++) 
        {
            if (weightedScenes[i].sceneName == SceneName)
            {
                weightedScenes[i].weight = newWeight;
            }
        } 
    }
    public void RemoveScene(string SceneName)
    {
        for (int i = 0; i < weightedScenes.Count; i++)
        {
            if (weightedScenes[i].sceneName == SceneName)
            {
                weightedScenes.Remove(weightedScenes[i]);
            }
        }
    }
    private IEnumerator UnloadSceneAsync(string sceneName)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
        while (!asyncUnload.isDone)
            yield return null;

        activeScenes.Remove(sceneName);
    }
}
public interface IGameSceneManager
{
    bool IsCurrentScene(string _sceneName);
    void LoadNextScene(string _sceneName);
    void LoadRandomScene();
    void UnloadLastScene();
    void ChangeSceneWeighth(string SceneName, int newWeight);
    void RemoveScene(string SceneName);
    IEnumerator LoadSceneAsync(string sceneName);
    void SetInitialLoop(bool isActive);
    bool GetIsInInitialLoop();
}