using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TimeManager timeManager;
    [SerializeField] private TextMeshProUGUI contador_provicional;
    [SerializeField] private TextMeshProUGUI uiText;
    private void Awake()
    {
        timeManager = FindFirstObjectByType<TimeManager>();
    }
    private void Start()
    {
        uiText.gameObject.SetActive(false);
    }
    void Update()
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
