using Unity.Cinemachine.Samples;
using UnityEngine;

namespace Player
{
    public class ObjectInHandState : IState
    {
        PlayerStateController controller;
        PlayerInputHandler input;
        PlayerMovement movement;
        CinemachinePOVExtension playerCamera;

        public ObjectInHandState(PlayerStateController controller, PlayerInputHandler input, PlayerMovement movement, CinemachinePOVExtension playerCamera)
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
            Debug.Log("Entering NormalState");
        }
        public void Execute()
        {
            throw new System.NotImplementedException();
        }
        public void Exit()
        {
            movement.CanMove = false;
            playerCamera.CanLook = false;
            Debug.Log("Exiting NormalState");
        }
    }
}