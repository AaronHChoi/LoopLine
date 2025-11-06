using System;
using System.Collections.Generic;
using System.Linq;
using DependencyInjection;
using TMPro;
using UnityEngine;

public enum PanelPosition
{
    Center,
    Left, 
    Right
}
[Serializable]
public class UIPanelEntry
{
    public string panelID;
    public UIPanelData panelData;
    public PanelPosition position = PanelPosition.Center;
}

public class UIManager : Singleton<UIManager>, IUIManager
{
    [SerializeField] private TextMeshProUGUI contador_provicional;
    [SerializeField] GameObject pauseManager;
    bool isCursorVisible = false;

    [Header("UI Panel Manager")]
    [SerializeField] List<UIPanelEntry> managedPanels = new List<UIPanelEntry>();
    [SerializeField] GameObject infoPanelObject;

    [Header("Panel positions (RectTransforms)")]
    [SerializeField] Transform centerPosition;
    [SerializeField] Transform leftPosition;
    [SerializeField] Transform rightPosition;

    GameObject currentActivePanel = null;
    InfoPanel panelScript;
    //[SerializeField] GameObject tutorialClockUI;

    ICrosshairFade crosshairFade;
    IGameStateController gameController;
    protected override void Awake()
    {
        base.Awake();

        gameController = InterfaceDependencyInjector.Instance.Resolve<IGameStateController>();
        crosshairFade = InterfaceDependencyInjector.Instance.Resolve<ICrosshairFade>();

        if (infoPanelObject != null)
        {
            infoPanelObject.SetActive(false);
            panelScript = infoPanelObject.GetComponent<InfoPanel>();
            if(panelScript == null)
            {
                Debug.LogError("infoPanelObject dont have the infoPanel script");
            }
        }
        else
        {
            Debug.LogError("UIManager dont have assigned infoPanelObject");
        }
    }
    private void OnEnable()
    {
        gameController.OnPauseMenu += PauseMenu;
    }
    private void OnDisable()
    {
        gameController.OnPauseMenu -= PauseMenu;
    }
    public void ShowPanel(string panelID)
    {
        UIPanelEntry entry = managedPanels.FirstOrDefault(p => p.panelID == panelID);

        if (entry == null)
        {
            Debug.LogWarning($"UIManager: No panel was found with the ID: {panelID}");
            return;
        }

        HideCurrentPanel();

        if (infoPanelObject  == null || panelScript == null)
        {
            Debug.LogError("The panel cannot be displayed, infoPanelObject is not setting");
            return;
        }
        
        if (entry.panelData != null)
        {
            panelScript.Setup(entry.panelData);
        }
        else
        {
            Debug.LogError($"The panel {panelID} does not have UIPanelData. it will be displayed empty");
        }

        ApplyPanelPosition(entry.position);

        infoPanelObject.SetActive(true);
        currentActivePanel = infoPanelObject;
    }
    private void ApplyPanelPosition(PanelPosition position)
    {
        Transform targetPosition = centerPosition;

        switch (position)
        {
            case PanelPosition.Left:
                targetPosition = leftPosition;
                break;
            case PanelPosition.Right:
                targetPosition = rightPosition;
                break;
            case PanelPosition.Center:
                targetPosition = centerPosition;
                break;
        }

        if (targetPosition != null)
        {
            infoPanelObject.transform.position = targetPosition.position;
        }
    }
    public void HideCurrentPanel()
    {
        if (currentActivePanel != null)
        {
            currentActivePanel.SetActive(false);
            currentActivePanel = null;
        }
    }
    public void PauseMenu()
    {
        bool isOpeningPause = !pauseManager.activeSelf;

        if (isOpeningPause)
        {
            HideCurrentPanel();
        }

        pauseManager.SetActive(isOpeningPause);
        UpdateCursorState();
    }
    public void ShowCrossHairFade(bool show)
    {
        //  crosshairFade.ShowCrosshair(show);
    }
    void UpdateCursorState()
    {
        bool shouldShowCursor = pauseManager.activeInHierarchy;

        if (isCursorVisible != shouldShowCursor)
        {
            isCursorVisible = shouldShowCursor;
            Cursor.visible = isCursorVisible;
            Cursor.lockState = isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
public interface IUIManager
{
    void ShowCrossHairFade(bool show);
    //void ShowClockTutorial(bool show);
    void ShowPanel(string panelID);
    void HideCurrentPanel();
}