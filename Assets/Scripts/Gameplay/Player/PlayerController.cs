using DependencyInjection;
using Player;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerStateController))]
public class PlayerController : MonoBehaviour, IPlayerController
{
    [SerializeField] PlayerModel playerModel;

    IPlayerMovement playerMovement;
    IPlayerStateController playerStateController;
    IGameSceneManager gameSceneManager;
    ISceneTransitionController sceneTransitionController;

    public PlayerModel PlayerModel => playerModel;

    private void Awake()
    {
        sceneTransitionController = InterfaceDependencyInjector.Instance.Resolve<ISceneTransitionController>();
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        playerMovement = InterfaceDependencyInjector.Instance.Resolve<IPlayerMovement>();
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
    }
    private void Update()
    {
        playerMovement.HandleMovement();
    }
    private void OnEnable()
    {
        if (playerStateController != null)
        {
            playerStateController.OnTeleport += TeleportPlayer;
        }
    }
    private void OnDisable()
    {
        if (playerStateController != null)
        {
            playerStateController.OnTeleport -= TeleportPlayer;
        }
    }
    private void TeleportPlayer()
    {
        if (gameSceneManager.IsCurrentScene("04. Train"))
        {
            sceneTransitionController.StartTransition(true);
            DelayUtility.Instance.Delay(2f, () =>
            {
                gameSceneManager.LoadNextScene("05. MindPlace");
            });
        }

        if (gameSceneManager.IsCurrentScene("05. MindPlace"))
        {
            sceneTransitionController.StartTransition(true);
            DelayUtility.Instance.Delay(2f, () =>
            {
                gameSceneManager.LoadNextScene("04. Train");
            });
        }
    }
    public Transform GetTransform()
    {
        return playerMovement.transform;
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
public interface IPlayerController
{
    Transform GetTransform();
    GameObject GetGameObject();
    public PlayerModel PlayerModel { get; }
}