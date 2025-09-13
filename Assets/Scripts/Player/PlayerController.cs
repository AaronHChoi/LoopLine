using DependencyInjection;
using Player;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerStateController))]
public class PlayerController : MonoBehaviour, IPlayerController
{
    [SerializeField] PlayerModel playerModel;

    private DialogueSpeaker dialogueSpeaker;

    IPlayerMovement playerMovement;

    public PlayerModel PlayerModel => playerModel;
    public DialogueSpeaker DialogueSpeaker => dialogueSpeaker;

    private void Awake()
    {
        playerMovement = InterfaceDependencyInjector.Instance.Resolve<IPlayerMovement>();
    }

    private void Start()
    {
        dialogueSpeaker = GetComponent<DialogueSpeaker>();
    }
    private void Update()
    {
        playerMovement.HandleMovement();
        playerMovement.RotateCharacterToCamera();
    }

    public Transform GetTransform()
    {
        return playerMovement.transform;
    }
}
public interface IPlayerController
{
    Transform GetTransform();
    public DialogueSpeaker DialogueSpeaker { get; }
    public PlayerModel PlayerModel { get; }
}