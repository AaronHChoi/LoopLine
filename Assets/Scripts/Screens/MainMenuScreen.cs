using UnityEngine;
using UnityEngine.UI;

public class MainMenuScreen : MonoBehaviour, IScreen
{
    [SerializeField] private Button playButton;
    private void Awake()
    {
        playButton.onClick.AddListener(() => GameManager.Instance.ScreenManager.Push(EnumScreenName.Gameplay));
    }
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void Free()
    {
        gameObject.SetActive(false);
    }
}
