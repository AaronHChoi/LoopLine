using Unity.Cinemachine.Samples;
using UnityEngine;

namespace Player
{
    public class InventoryState : IState
    {
        PlayerStateController controller;
        PlayerInputHandler input;
        PlayerMovement movement;
        CinemachinePOVExtension playerCamera;

        public InventoryState(PlayerStateController controller, PlayerInputHandler input, PlayerMovement movement, CinemachinePOVExtension playerCamera)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
            this.playerCamera = playerCamera;
        }
        public void Enter()
        {
            movement.CanMove = true;
            playerCamera.CanLook = true;
            Debug.Log("Entering InventoryState");
        }
        public void Execute()
        {
            if (input.OpenInventoryPressed())
            {
                controller.UseEventOpenInventory();
                controller.ChangeState(controller.NormalState);
            }
            if (input.InteractPressed())
            {
                controller.UseEventInteract();
            }
        }
        public void Exit()
        {
            movement.CanMove = false;
            playerCamera.CanLook = false;
            Debug.Log("Exiting InventoryState");
        }
    }
}