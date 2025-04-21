using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TimeManager timeManager;
    [SerializeField] private TextMeshProUGUI contador_provicional;
    private void Awake()
    {
        timeManager = FindFirstObjectByType<TimeManager>();
    }
    void Update()
    {
        contador_provicional.text = GetFormattedLoopTime(timeManager.LoopTime);
    }
    public string GetFormattedLoopTime(float loopTime)
    {
        return Mathf.FloorToInt(loopTime).ToString();
    }
}
