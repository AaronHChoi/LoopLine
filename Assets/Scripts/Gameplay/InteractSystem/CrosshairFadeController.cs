using UnityEngine;
using Core.UI;
using Core.DependencyInjection;

public class CrosshairFadeController : MonoBehaviour, ICrosshairFade
{
    IFadeInOutController fadeCrosshairBig;
    IFadeInOutController fadeCrosshairSmall;
    [SerializeField] private RaycastController rayController;

    private bool bigCroshairVisibility = false;
    private bool smallCroshairVisibility = false;

    string ignoreTag = "PhotoQuest";

    private void Awake()
    {
        fadeCrosshairBig = InterfaceDependencyInjector.Instance.Resolve<IFadeInOutController>(FadeID.CrosshairBig);
        fadeCrosshairSmall = InterfaceDependencyInjector.Instance.Resolve<IFadeInOutController>(FadeID.CrosshairSmall);
    }
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        bool showInteractCrosshair = false;

        if (rayController.FoundInteract)
        {
            if (rayController.Target != null && !rayController.Target.CompareTag(ignoreTag))
            {
                showInteractCrosshair = true;
            }
        }

        if (showInteractCrosshair)
        {
            ShowCrosshairByFade(fadeCrosshairBig, ref bigCroshairVisibility, true);
            ShowCrosshairByFade(fadeCrosshairSmall, ref smallCroshairVisibility, false);
        }
        else
        {
            ShowCrosshairByFade(fadeCrosshairBig, ref bigCroshairVisibility, false);
            ShowCrosshairByFade(fadeCrosshairSmall, ref smallCroshairVisibility, true);
        }
    }
    private void ShowCrosshairByFade(IFadeInOutController fade, ref bool visibility, bool show)
    {
        //Fades if it can be fade and save it's state
        if (visibility == show) return;
        else visibility = show;

        if (fade == null) return;

        fade.ForceFade(show);
    }
    public void ShowCrosshair(bool show)
    {
        if (show)
        {
            ShowCrosshairByFade(fadeCrosshairBig, ref bigCroshairVisibility, false);
            ShowCrosshairByFade(fadeCrosshairSmall, ref smallCroshairVisibility, true);
        }
        else
        {
            ShowCrosshairByFade(fadeCrosshairBig, ref bigCroshairVisibility, false);
            ShowCrosshairByFade(fadeCrosshairSmall, ref smallCroshairVisibility, false);
        }
    }
}

public interface ICrosshairFade
{
    void ShowCrosshair(bool show);
}