using DependencyInjection;
using TMPro;
using UnityEngine;
public class UIManager : MonoBehaviour, IUIManager
{
    [SerializeField] private TextMeshProUGUI contador_provicional;
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] GameObject pauseManager;
    bool isCursorVisible = false;

    IGameSceneManager gameSceneManager;
    ITimeProvider timeManager;
    ICrosshairFade crosshairFade;
    IGameStateController gameController;
    private void Awake()
    {
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
}