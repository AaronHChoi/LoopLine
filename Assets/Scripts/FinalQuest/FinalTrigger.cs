using DependencyInjection;
using Player;
using UnityEngine;

public class FinalTrigger : MonoBehaviour
{
    IGameSceneManager gameSceneManager;

    private void Awake()
    {
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        gameSceneManager.LoadNextScene("01. MainMenu");
    }
}