using Core.Audio;
using Core.DependencyInjection;
using Core.UI;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public GameObject UIPanel;
    public string panelTitle;

    public IFadeInOutController fade;
    private void Awake()
    {
        fade = InterfaceDependencyInjector.Instance.Resolve<IFadeInOutController>(FadeID.MenuPanel);
    }
}
