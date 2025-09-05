using UnityEngine;
using DependencyInjection;
public class PlayerFocusMode : MonoBehaviour, IDependencyInjectable
{
    [SerializeField] PlayerController controller;
    IColliderToggle focusModeManager;
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
        focusModeManager = InterfaceDependencyInjector.Instance.Resolve<IColliderToggle>();
    }
    public void ToggleFocusMode()
    {
        controller.PlayerModel.FocusMode = controller.PlayerModel.FocusMode;

        focusModeManager.ToggleColliders(controller.PlayerModel.FocusMode);
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        controller = provider.PlayerContainer.PlayerController;
    }
}