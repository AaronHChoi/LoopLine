using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private CharacterController characterController;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    public void Move(Vector3 movement)
    {
        characterController.Move(movement);
    }
}
