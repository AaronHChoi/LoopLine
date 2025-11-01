using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : Singleton<GameSceneManager>, IGameSceneManager
{
    [Header("Scene Settings")]
    [SerializeField] private WeightScene firstScene;
    [SerializeField] private List<WeightScene> weightedScenes = new List<WeightScene>();

    [Header("Active Scenes")]
    [SerializeField] private List<string> activeScenes = new List<string>();
    [SerializeField] private List <string> constantActiveScenes = new List<string>();

    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {     
        if (IsCurrentScene("04. Train"))
        {
            foreach (var sceneName in constantActiveScenes)
            {
                if (!SceneManager.GetSceneByName(sceneName).isLoaded)
                {
                    SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                }
            }
            StartCoroutine(LoadSceneAsync(firstScene.sceneName));
        }
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
    private IEnumerator LoadSceneAsync(string sceneName)
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
}