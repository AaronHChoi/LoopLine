using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI contador_provicional;
    void Update()
    {
        contador_provicional.text = GetFormattedLoopTime(GameManager.Instance.LoopTime);
    }

    public string GetFormattedLoopTime(float loopTime)
    {
        return Mathf.FloorToInt(loopTime).ToString();
    }
}
