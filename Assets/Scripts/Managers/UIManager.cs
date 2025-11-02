using DependencyInjection;
using TMPro;
using UnityEngine;
public class UIManager : Singleton<UIManager>, IUIManager
{
    [SerializeField] private TextMeshProUGUI contador_provicional;
    [SerializeField] GameObject pauseManager;
    bool isCursorVisible = false;
    [SerializeField] GameObject tutorialClockUI;

    ICrosshairFade crosshairFade;
    IGameStateController gameController;
    protected override void Awake()
    {
        base.Awake();

        gameController = InterfaceDependencyInjector.Instance.Resolve<IGameStateController>();
        crosshairFade = InterfaceDependencyInjector.Instance.Resolve<ICrosshairFade>();
    }
    private void OnEnable()
    {
        gameController.OnPauseMenu += PauseMenu;
    }
    private void OnDisable()
    {
        gameController.OnPauseMenu -= PauseMenu;
    }
    public void ShowClockTutorial(bool show)
    {
        if(tutorialClockUI != null)
        {
            tutorialClockUI.SetActive(show);
        }
    }
    public void PauseMenu()
    {
        pauseManager.SetActive(!pauseManager.activeSelf);
        UpdateCursorState();
    }
    public void ShowCrossHairFade(bool show)
    {
        crosshairFade.ShowCrosshair(show);
    }
    void UpdateCursorState()
    {
        bool shouldShowCursor = pauseManager.activeInHierarchy;

        if (isCursorVisible != shouldShowCursor)
        {
            isCursorVisible = shouldShowCursor;
            Cursor.visible = isCursorVisible;
            Cursor.lockState = isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
public interface IUIManager
{
    void ShowCrossHairFade(bool show);
    void ShowClockTutorial(bool show);
}