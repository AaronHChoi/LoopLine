using TMPro;
using UnityEngine;
using DependencyInjection;
public class UIManager : MonoBehaviour, IDependencyInjectable, IUIManager
{
    [SerializeField] private TextMeshProUGUI contador_provicional;
    [SerializeField] private TextMeshProUGUI uiText;

    GameSceneManager gameSceneManager;
    ITimeProvider timeManager;
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
        timeManager = InterfaceDependencyInjector.Instance.Resolve<ITimeProvider>();
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        gameSceneManager = provider.ManagerContainer.GameSceneManager;
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
}
public interface IUIManager
{
    void ShowUIText(string _message);
    void HideUIText();
}