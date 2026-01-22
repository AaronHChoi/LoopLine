using UnityEngine;
using Core.DependencyInjection;

public class FinalTrigger : MonoBehaviour
{
    IGameSceneManager gameSceneManager;

    private void Start()
    {
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        gameSceneManager.LoadNextScene("01. MainMenu");
    }
}