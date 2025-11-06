using DependencyInjection;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    IPlayerInputHandler inputHandler;
    IPlayerController controller;
    IPlayerCamera playerCamera;

    private void Awake()
    {
        inputHandler = InterfaceDependencyInjector.Instance.Resolve<IPlayerInputHandler>();
        controller = InterfaceDependencyInjector.Instance.Resolve<IPlayerController>();
        playerCamera = InterfaceDependencyInjector.Instance.Resolve<IPlayerCamera>();
    }
    private void LateUpdate()
    {
        PlayerModel model = controller.PlayerModel;
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