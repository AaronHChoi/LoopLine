using TMPro;
using UnityEngine;
using DependencyInjection;
public class UIManager : MonoBehaviour, IUIManager
{
    [SerializeField] private TextMeshProUGUI contador_provicional;
    [SerializeField] private TextMeshProUGUI uiText;

    IGameSceneManager gameSceneManager;
    ITimeProvider timeManager;
    ICrosshairFade crosshairFade;
    private void Awake()
    {
        timeManager = InterfaceDependencyInjector.Instance.Resolve<ITimeProvider>();
        crosshairFade = InterfaceDependencyInjector.Instance.Resolve<ICrosshairFade>();
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
    }
    private void Start()
    {
        uiText.gameObject.SetActive(false);
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
}
public interface IUIManager
{
    void ShowUIText(string _message);
    void HideUIText();
    void ShowCrossHairFade(bool show);
}