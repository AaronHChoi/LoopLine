using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour, IDependencyInjectable
{
    [SerializeField] private TextMeshProUGUI contador_provicional;
    [SerializeField] private TextMeshProUGUI uiText;

    GameSceneManager gameSceneManager;
    TimeManager timeManager;
    private void Awake()
    {
        InjectDependencies(DependencyContainer.Instance);
    }
    public void InjectDependencies(DependencyContainer provider)
    {
        gameSceneManager = provider.GameSceneManager;
        timeManager = provider.TimeManager;
    }
    private void Start()
    {
        uiText.gameObject.SetActive(false);
    }
    void Update()
    {
        if (gameSceneManager != null && gameSceneManager.IsCurrentScene("Train"))
        {
            ShowLoopTime();
        }
    }
    private void ShowLoopTime()
    {
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
