using DependencyInjection;
using UnityEngine;

public class TeleportLoop : MonoBehaviour
{
    public Transform TeleportZoneObject;

    [SerializeField] private DoorInteract doorInteract;
    IGameSceneManager gameSceneManager;
    IPhotoCapture polaroid;

    [SerializeField] GameObject player;

    private void Awake()
    {
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
        polaroid = InterfaceDependencyInjector.Instance.Resolve<IPhotoCapture>();
    }
    public void Teleport()
    {
        Vector3 localOffset = transform.InverseTransformPoint(player.transform.position);
        Quaternion relativeRotation = TeleportZoneObject.rotation * Quaternion.Inverse(transform.rotation);
        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null)
        {
            gameSceneManager.UnloadLastScene();
            gameSceneManager.LoadRandomScene();

            cc.enabled = false;

            player.transform.position = TeleportZoneObject.TransformPoint(localOffset);
            player.transform.rotation = relativeRotation * player.transform.rotation;

            cc.enabled = true;

            GameManager.Instance.TrainLoop += 1;
            polaroid.ResetPhotoCounter();
        }
    }
}