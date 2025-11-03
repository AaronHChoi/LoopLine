using System;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairFadeController : MonoBehaviour, ICrosshairFade
{
    [SerializeField] private FadeInOutController fadeCrosshairBig;
    [SerializeField] private FadeInOutController fadeCrosshairSmall;
    [SerializeField] private RaycastController rayController;

    private bool bigCroshairVisibility = false;
    private bool smallCroshairVisibility = false;


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        if (rayController.FoundInteract)
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
    private void ShowCrosshairByFade(FadeInOutController fade, ref bool visibility, bool show)
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