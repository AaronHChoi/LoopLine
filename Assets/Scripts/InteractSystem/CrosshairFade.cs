using UnityEngine;
using UnityEngine.UI;

public class CrosshairFade : MonoBehaviour, ICrosshairFade
{
    [SerializeField] private RawImage crosshairImage;
    [SerializeField] private RaycastController rayController;

    private FadeInOutController crossFade;
    private bool isVisible = true;


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SetCrosshairVisible(false); // hide crosshair at start
        if (crosshairImage != null) crossFade = crosshairImage.GetComponent<FadeInOutController>();
    }

    void Update()
    {
        if (rayController.FoundInteract)
        {
            SetCrosshairOpacity(rayController.BestScore); // fade based on angle + distance
            SetCrosshairVisible(true);
        }
        else
        {
            SetCrosshairVisible(false);
        }
    }

    private void SetCrosshairVisible(bool visible)
    {
        crosshairImage.enabled = visible;
    }

    private void SetCrosshairOpacity(float opacity)
    {
        Color color = crosshairImage.color;
        opacity = Mathf.Clamp01(opacity);
        color.a = opacity;
        crosshairImage.color = color;
    }
    public void ShowCrosshair(bool show)
    {
        //Fades if it can be fade and save it's state
        if (isVisible == show) return;
        else isVisible = show;

        if (crossFade == null) return;

        crossFade.ForceFade(!show);
    }
}

public interface ICrosshairFade
{
    void ShowCrosshair(bool show);
}