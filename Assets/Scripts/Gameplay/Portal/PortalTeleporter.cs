using DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PortalTeleporter : MonoBehaviour {

	public IPlayerController player;
	public Transform reciever;

	private bool playerIsOverlapping = false;

    private void Awake()
    {
        player = InterfaceDependencyInjector.Instance.Resolve<IPlayerController>();
    }
    // Update is called once per frame
    void Update () {
		if (playerIsOverlapping)
		{
			Vector3 portalToPlayer = player.GetTransform().position - transform.position;
			float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

			// If this is true: The player has moved across the portal
			if (dotProduct < 0f)
			{
				CharacterController PlayerController = player.GetGameObject().GetComponent<CharacterController>();
				PlayerController.enabled = false;
				// Teleport him!
				float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
				rotationDiff += 180;
				player.GetTransform().Rotate(Vector3.up, rotationDiff);

				Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
				player.GetTransform().position = reciever.position + positionOffset;
				PlayerController.enabled = true;
				playerIsOverlapping = false;
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player")
		{
			playerIsOverlapping = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player")
		{
			playerIsOverlapping = false;
		}
	}
}
