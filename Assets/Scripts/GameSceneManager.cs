using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour,IDependencyInjectable
{
    DialogueManager dialogueManager;
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        dialogueManager = provider.DialogueManager;
    }
    public void LoadNextScene(string _sceneName)
    {
        if(_sceneName == "MindPlace")
        {
            GameManager.Instance.TrainLoop++;
            dialogueManager.ResetAllDialogues();
        }
        SceneManager.LoadScene(_sceneName);
    }
    public bool IsCurrentScene(string _sceneName)
    {
        return SceneManager.GetActiveScene().name == _sceneName;
    }
}
