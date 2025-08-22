using UnityEngine;

namespace Player
{
    public class InventoryState : IState
    {
        PlayerStateController controller;
        PlayerInputHandler input;
        PlayerMovement movement;

        public InventoryState(PlayerStateController controller, PlayerInputHandler input, PlayerMovement movement)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
        }
        public void Enter()
        {
            movement.CanMove = true;
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
            movement.CanMove = true;
            Debug.Log("Exiting InventoryState");
        }
    }
}