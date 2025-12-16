using DependencyInjection;
using UnityEngine;

public class FinalDoor : MonoBehaviour, IInteract
{
    //private bool isCursorVisible = false;
    IGameSceneManager gameSceneManager;

    private void Awake()
    {
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
    }
    public string GetInteractText()
    {
        return null;
    }
    public void Interact()
    {
        //if (GameManager.Instance.GetCondition(GameCondition.IsPhotoQuestComplete))
        //{
        //    gameSceneManager.LoadNextScene("01. MainMenu");
        //}
    }

}