using UnityEngine;
using UnityEngine.SceneManagement;
using DependencyInjection;
public class GameSceneManager : MonoBehaviour, IGameSceneManager
{
    IDialogueManager dialogueManager;
    private void Awake()
    {
        dialogueManager = InterfaceDependencyInjector.Instance.Resolve<IDialogueManager>();
    }
    public void LoadNextScene(string _sceneName)
    {
        if(_sceneName == "05. MindPlace")
        {
            GameManager.Instance.TrainLoop++;
            dialogueManager.ResetAllDialogues();
            dialogueManager.UnlockFirstDialogues();
        }
        SceneManager.LoadScene(_sceneName);
    }
    public bool IsCurrentScene(string _sceneName)
    {
        return SceneManager.GetActiveScene().name == _sceneName;
    }
}

public interface IGameSceneManager
{
    bool IsCurrentScene(string _sceneName);
    void LoadNextScene(string _sceneName);
}
