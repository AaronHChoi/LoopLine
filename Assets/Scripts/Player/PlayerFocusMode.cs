using UnityEngine;

public class PlayerFocusMode : MonoBehaviour
{
    IColliderToggle focusModeManager;
    private void Awake()
    {
        focusModeManager = InterfaceDependencyInjector.Instance.Resolve<IColliderToggle>();
    }
    public void ToggleFocusMode(PlayerModel _playerModel)
    {
        _playerModel.FocusMode = !_playerModel.FocusMode;

        focusModeManager.ToggleColliders(_playerModel.FocusMode);
    }
}