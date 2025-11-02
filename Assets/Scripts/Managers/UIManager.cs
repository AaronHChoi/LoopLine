using DependencyInjection;
using TMPro;
using UnityEngine;
public class UIManager : Singleton<UIManager>, IUIManager
{
    [SerializeField] private TextMeshProUGUI contador_provicional;
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] GameObject pauseManager;
    bool isCursorVisible = false;
    [SerializeField] GameObject tutorialClockUI;

    IGameSceneManager gameSceneManager;
    ITimeProvider timeManager;
    ICrosshairFade crosshairFade;
    IGameStateController gameController;
    protected override void Awake()
    {
        base.Awake();

        timeManager = InterfaceDependencyInjector.Instance.Resolve<ITimeProvider>();
        gameController = InterfaceDependencyInjector.Instance.Resolve<IGameStateController>();
        crosshairFade = InterfaceDependencyInjector.Instance.Resolve<ICrosshairFade>();
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
    }
    private void Start()
    {
        uiText.gameObject.SetActive(false);
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
    void Update()
    {
        if (gameSceneManager != null && gameSceneManager.IsCurrentScene("04. Train"))
        {
            ShowLoopTime();
        }
    }
    private void ShowLoopTime()
    {
        if(timeManager != null)
            contador_provicional.text = GetFormattedLoopTime(timeManager.LoopTime);
    }
    public string GetFormattedLoopTime(float loopTime)
    {
        return Mathf.FloorToInt(loopTime).ToString();
    }
    public void ShowUIText(string _message)
    {
        uiText.gameObject.SetActive(true);
        uiText.text = _message;
    }
    public void HideUIText()
    {
        uiText.gameObject.SetActive(false);
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
    void ShowUIText(string _message);
    void HideUIText();
    void ShowCrossHairFade(bool show);
    void ShowClockTutorial(bool show);
}