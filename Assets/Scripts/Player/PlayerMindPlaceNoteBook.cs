using UnityEngine;
using DependencyInjection;
public class PlayerMindPlaceNoteBook : MonoBehaviour
{
    INoteBookColliderToggle noteBookManager;
    private void Awake()
    {
        noteBookManager = InterfaceDependencyInjector.Instance.Resolve<INoteBookColliderToggle>();
    }
    public void ToggleNooteBook(PlayerModel _playerModel)
    {
        //_playerModel.IsNoteBookOpen = !_playerModel.IsNoteBookOpen;

        noteBookManager.ToggleColliders(_playerModel.IsNoteBookOpen);
    }
}
