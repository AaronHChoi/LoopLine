using Core.Audio;
using Core.DependencyInjection;
using Core.UI;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public GameObject UIPanel;
    public string panelTitle;

    [SerializeField] public IFadeInOutController fade;

    [SerializeField] public FadeID fadeID;

    public void Fade(bool Fade)
    {
        fade.ForceFade(Fade);
    }
}
