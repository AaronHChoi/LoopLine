using UnityEngine;

namespace Player
{
    public class NormalState : IState
    {
        IPlayerStateController controller;
        IPlayerInputHandler input;
        IPlayerMovement movement;
        ICameraOrientation playerCameraOrientation;
        IUIManager uiManager;
        IPlayerCamera playerCamera;
        IPlayerController playerController;
        IGameSceneManager gameSceneManager;

        public NormalState(IPlayerStateController controller, IPlayerInputHandler input, 
            IPlayerMovement movement, ICameraOrientation playerCameraOrientation, IUIManager uiManager,
            IPlayerController playerController, IPlayerCamera playerCamera, IGameSceneManager gameSceneManager)
        {
            this.controller = controller;
            this.input = input;
            this.movement = movement;
            this.playerCameraOrientation = playerCameraOrientation;
            this.uiManager = uiManager;
            this.playerController = playerController;
            this.playerCamera = playerCamera;
            this.gameSceneManager = gameSceneManager;
        }
        public void Enter()
        {
            movement.CanMove = true;
            playerCameraOrientation.CanLook = true;
            Debug.Log("Entering NormalState");
        }
        public void Execute()
        {
            HandleHeadBob();

            if (GameManager.Instance.GetCondition(GameCondition.PolaroidTaken) && input.ToggleCameraPressed() && gameSceneManager.IsCurrentScene("04. Train"))
            {
                controller.StateMachine.TransitionTo(controller.CameraState);
            }
            if (input.InteractPressed())
            {
                controller.UseEventInteract();
            }
            if (input.GrabItemPressed())
            {
                controller.UseEventGrab();
            }
            if (input.OpenInventoryPressed())
            {
                controller.UseEventOpenInventory();
            }
            if (input.DevelopmentModePressed())
            {
                controller.UseEventDevelopment();
                controller.StateMachine.TransitionTo(controller.DevelopmentState);
            }
            if (GameManager.Instance.GetCondition(GameCondition.TeleportAvailable))
            {
                if (input.Teleport())
                {
                    EventBus.Publish(new TransitionEvent());
                    uiManager.HideCurrentPanel();
                    controller.UseEventTeleport();
                }
            }
   
        }
        public void Exit()
        {
            movement.CanMove = false;
            playerCameraOrientation.CanLook = false;

            Debug.Log("Exiting NormalState");
        }
        void HandleHeadBob()
        {
            PlayerModel model = playerController.PlayerModel;
            Vector2 moveInput = input.GetInputMove();

            float horizontalFactor = 0.2f;

            if (moveInput.magnitude > 0.1f)
            {
                bool isSprinting = input.IsSprinting();

                if (isSprinting)
                {
                    playerCamera.ApplyHeadBob(model.SprintBobFrequencyGain, model.SprintBobAmplitudeGain, horizontalFactor);
                }
                else
                {
                    playerCamera.ApplyHeadBob(model.WalkBobFrequencyGain, model.WalkBobAmplitudeGain, horizontalFactor);
                }
            }
            else
            {
                playerCamera.ResetHeadBob(model.BobSmoothTime);
            }
        }
    }
}