using UnityEngine;

public class TeleportLoop : MonoBehaviour
{
    public Transform TeleportZoneObject;

    [SerializeField] GameObject player;

    public void Teleport()
    {
        Vector3 localOffset = transform.InverseTransformPoint(player.transform.position);
        Quaternion relativeRotation = TeleportZoneObject.rotation * Quaternion.Inverse(transform.rotation);
        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null)
        {
            EventBus.Publish(new LoopTeleportEvent());

            cc.enabled = false;

            player.transform.position = TeleportZoneObject.TransformPoint(localOffset);
            player.transform.rotation = relativeRotation * player.transform.rotation;

            cc.enabled = true;
        }
    }
}