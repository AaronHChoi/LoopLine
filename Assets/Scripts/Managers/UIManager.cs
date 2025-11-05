using System;
using System.Collections.Generic;
using System.Linq;
using DependencyInjection;
using TMPro;
using UnityEngine;

[Serializable]
public class UIPanelEntry
{
    public string panelID;
    public GameObject panelGameObject;
    public UIPanelData panelData;
}

public class UIManager : Singleton<UIManager>, IUIManager
{
    [SerializeField] private TextMeshProUGUI contador_provicional;
    [SerializeField] GameObject pauseManager;
    bool isCursorVisible = false;

    [Header("UI Panel Manager")]
    [SerializeField] List<UIPanelEntry> managedPanels = new List<UIPanelEntry>();

    GameObject currentActivePanel = null;
    //[SerializeField] GameObject tutorialClockUI;

    ICrosshairFade crosshairFade;
    IGameStateController gameController;
    protected override void Awake()
    {
        base.Awake();

        gameController = InterfaceDependencyInjector.Instance.Resolve<IGameStateController>();
        crosshairFade = InterfaceDependencyInjector.Instance.Resolve<ICrosshairFade>();

        foreach(var entry in managedPanels)
        {
            if(entry.panelGameObject != null)
            {
                entry.panelGameObject.SetActive(false);
            }
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
    //public void ShowClockTutorial(bool show)
    //{
    //    if(tutorialClockUI != null)
    //    {
    //        tutorialClockUI.SetActive(show);
    //    }
    //}
    public void ShowPanel(string panelID)
    {
        UIPanelEntry entry = managedPanels.FirstOrDefault(p => p.panelID == panelID);

        if (entry == null)
        {
            Debug.LogWarning($"UIManager: No panel was found with the ID: {panelID}");
            return;
        }

        HideCurrentPanel();

        if (entry.panelGameObject != null)
        {
            InfoPanel panelScript = entry.panelGameObject.GetComponent<InfoPanel>();
            if (panelScript != null && entry.panelData != null)
            {
                panelScript.Setup(entry.panelData);
            }
            else
            {
                Debug.LogWarning($"The {panelID} panel does no have an infoPanel script or is missing UIPanelData. It will only be activated");
            }
            entry.panelGameObject.SetActive(true);
            currentActivePanel = entry.panelGameObject;
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