using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mainTextComponent;

    public void Setup(UIPanelDataSO data)
    {
        if (mainTextComponent != null)
        {
            mainTextComponent.text = data.MainText;
        }
    }
}
