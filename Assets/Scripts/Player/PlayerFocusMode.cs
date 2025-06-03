using UnityEngine;

public class PlayerFocusMode : MonoBehaviour, IDependencyInjectable
{
    FocusModeManager focusModeManager;
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        focusModeManager = provider.FocusModeManager;
    }
    public void ToggleFocusMode(PlayerModel _playerModel)
    {
        _playerModel.FocusMode = !_playerModel.FocusMode;

        focusModeManager.ToggleColliders(_playerModel.FocusMode);
    }
}
