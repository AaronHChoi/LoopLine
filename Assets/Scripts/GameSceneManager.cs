using System.Collections;
using System.Collections.Generic;
using DependencyInjection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : Singleton<GameSceneManager>, IGameSceneManager
{
    [Header("Scene Settings")]
    [SerializeField] private WeightScene firstScene;
    [SerializeField] private WeightScene secondScene;
    [SerializeField] private List<WeightScene> weightedScenes = new List<WeightScene>();

    [Header("Active Scenes")]
    [SerializeField] private List<string> activeScenes = new List<string>();

    bool isInInitialLoop = false;

    IMonologueSpeaker monologueSpeaker;
    ISceneWeightController weightController;
    protected override void Awake()
    {
        base.Awake();

        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
        weightController = InterfaceDependencyInjector.Instance.Resolve<ISceneWeightController>();
    }
    private void Start()
    {
        if (GameManager.Instance.GetCondition(GameCondition.PolaroidTaken) && IsCurrentScene("04. Train"))
        {
            StartCoroutine(LoadSceneAsync(firstScene.sceneName));
            return;
        }
        if (GameManager.Instance.GetCondition(GameCondition.PhotoDoorOpen) && IsCurrentScene("04. Train"))
        {
            StartCoroutine(LoadSceneAsync(secondScene.sceneName));
            weightController.HandleConditionChanged(GameCondition.PhotoDoorOpen, true);
            return;
        }
        if (IsCurrentScene("04. Train"))
        {
            StartCoroutine(LoadSceneAsync(firstScene.sceneName));
            return;
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

        WeightScene sceneData = GetWeightScene(selectedScene);
        sceneData.TimesLoaded++;

        StartMonologueByTimesInScene(selectedScene, sceneData, 1);
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
        float totalDynamicWeight = 0f;

        foreach (var scene in weightedScenes)
        {
            float dynamicWeight = scene.weight / (scene.TimesLoaded + 1);
            totalDynamicWeight += dynamicWeight;
        }
        //float totalWeight = 0f;
        //foreach (var scene in weightedScenes)
        //    totalWeight += scene.weight;

        float randomValue = Random.value * totalDynamicWeight;
        float current = 0f;

        foreach (var scene in weightedScenes)
        {
            float dynamicWeight = scene.weight / (scene.TimesLoaded + 1);
            current += dynamicWeight;

            if (randomValue <= current)
            {
                return scene.sceneName;
            }
        }
        return weightedScenes[0].sceneName;
    }
    public WeightScene GetWeightScene(string sceneName)
    {
        foreach (var scene in weightedScenes)
        {
            if (scene.sceneName == sceneName)
                return scene;
        }
        return null;
    }
    public void LoadSceneAsync2(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
        WeightScene sceneData = GetWeightScene(sceneName);
        sceneData.TimesLoaded++;

        StartMonologueByTimesInScene(sceneName, sceneData, 1);
    }
    private void StartMonologueByTimesInScene(string sceneName, WeightScene sceneData, int TimestoLoad)
    {
        if (monologueSpeaker != null)
        {
            if (sceneData.TimesLoaded == TimestoLoad)
            {
                DelayUtility.Instance.Delay(2f, () =>
                {
                    if (monologueSpeaker != null)
                    {
                        if (GameManager.Instance.GetCondition(sceneData.sceneCondition) == GameManager.Instance.GetCondition(GameCondition.None))
                        {
                            monologueSpeaker.StartMonologue(sceneData.SceneEvent);
                        }
                        if (!GameManager.Instance.GetCondition(sceneData.sceneCondition))
                        {
                            GameManager.Instance.SetCondition(sceneData.sceneCondition, true);
                        }
                    }
                });
            }
        }
    }
    public IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
            yield return null;
        activeScenes.Add(sceneName);
    }
    private IEnumerator WaitforTime(float time)
    {
        if (time < 0f)
            time = 0f;
        yield return new WaitForSeconds(time);
        yield break;
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
    public WeightScene GetWeightScene(string sceneName);
    void LoadSceneAsync2(string sceneName);
}