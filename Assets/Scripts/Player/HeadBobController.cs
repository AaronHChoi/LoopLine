using DependencyInjection;
using Player;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    IPlayerInputHandler inputHandler;
    IPlayerController controller;
    IPlayerCamera playerCamera;
    IPlayerStateController playerStateController;

    private void Awake()
    {
        inputHandler = InterfaceDependencyInjector.Instance.Resolve<IPlayerInputHandler>();
        controller = InterfaceDependencyInjector.Instance.Resolve<IPlayerController>();
        playerCamera = InterfaceDependencyInjector.Instance.Resolve<IPlayerCamera>();
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
    }
    private void LateUpdate()
    {
        PlayerModel model = controller.PlayerModel;

        if (playerStateController.IsInState(playerStateController.DialogueState))
        {
            playerCamera.SetNoiseGains(0, 0, model.BobSmoothTime);
            return;
        }

        Vector2 moveInput = inputHandler.GetInputMove();

        if (moveInput.magnitude > 0.1f)
        {
            bool isSprinting = inputHandler.IsSprinting();

            if (isSprinting)
            {
                playerCamera.SetNoiseGains(model.SprintBobAmplitudeGain, model.SprintBobFrequencyGain, model.BobSmoothTime);
            }
            else
            {
                playerCamera.SetNoiseGains(model.WalkBobAmplitudeGain, model.WalkBobFrequencyGain, model.BobSmoothTime);
            }
        }
        else
        {
            playerCamera.SetNoiseGains(0, 0, model.BobSmoothTime);
        }
    }
}