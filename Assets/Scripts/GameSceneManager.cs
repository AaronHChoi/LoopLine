using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    IDialogueResettable dialogueManager;
    private void Awake()
    {
        dialogueManager = InterfaceDependencyInjector.Instance.Resolve<IDialogueResettable>();
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
