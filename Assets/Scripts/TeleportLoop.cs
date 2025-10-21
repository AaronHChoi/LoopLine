using DependencyInjection;
using UnityEngine;

public class TeleportLoop : MonoBehaviour
{
    public Transform TeleportZoneObject;

    [SerializeField] private DoorInteract doorInteract;
    IGameSceneManager gameSceneManager;

    [SerializeField] GameObject player;

    private void Awake()
    {
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
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
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Vector3 localOffset = transform.InverseTransformPoint(other.transform.position);

    //        Quaternion relativeRotation = TeleportZoneObject.rotation * Quaternion.Inverse(transform.rotation);

    //        CharacterController cc = other.GetComponent<CharacterController>();

    //        if (cc != null)
    //        {
    //            gameSceneManager.UnloadLastScene();
    //            gameSceneManager.LoadRandomScene();

    //            cc.enabled = false;

    //            other.transform.position = TeleportZoneObject.TransformPoint(localOffset);

    //            other.transform.rotation = relativeRotation * other.transform.rotation;

    //            cc.enabled = true;
    //        }
    //    }
    //}
}