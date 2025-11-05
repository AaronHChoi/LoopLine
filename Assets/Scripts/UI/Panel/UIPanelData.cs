using UnityEngine;

[CreateAssetMenu(fileName = "NewUIPanelData", menuName = "UI/Panel Data")]
public class UIPanelData : ScriptableObject
{
    [TextArea(3, 10)]
    [SerializeField]string mainText;
    public string MainText => mainText;
}