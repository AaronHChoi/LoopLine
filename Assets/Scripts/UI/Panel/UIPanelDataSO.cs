using System;
using UnityEngine;

public enum PanelClose
{
    Force,
    Time
}

[CreateAssetMenu(fileName = "NewUIPanelData", menuName = "UI/Panel Data")]
public class UIPanelDataSO : ScriptableObject
{
    [SerializeField] PanelClose howToClose = PanelClose.Force;
    [SerializeField] float closeTime;
    [SerializeField] float offSetY;
    [SerializeField] float offSetX;
    [TextArea(3, 10)]
    [SerializeField]string mainText;

    public PanelClose HowToClose => howToClose;
    public float CloseTime => closeTime;
    public float OffSetY => offSetY;
    public float OffSetX => offSetX;
    public string MainText => mainText;
}