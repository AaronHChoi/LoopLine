using System;
using System.Collections;
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
    public UIPanelID panelID;
    public UIPanelDataSO panelData;
    public PanelPosition position = PanelPosition.Center;
}

public class UIManager : Singleton<UIManager>, IUIManager
{
    [SerializeField] private TextMeshProUGUI contador_provicional;
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

    Coroutine activeCloseCoroutine = null;

    //IPauseMenuManager pauseManager;
    ICrosshairFade crosshairFade;
    IGameStateController gameController;

    #region MAGIC_METHODS
    protected override void Awake()
    {
        base.Awake();

        gameController = InterfaceDependencyInjector.Instance.Resolve<IGameStateController>();
        crosshairFade = InterfaceDependencyInjector.Instance.Resolve<ICrosshairFade>();
        //pauseManager = InterfaceDependencyInjector.Instance.Resolve<IPauseMenuManager>();

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
    #endregion
    #region UI_TEXT
    public void ShowPanel(UIPanelID panelID)
    {
        UIPanelEntry entry = managedPanels.FirstOrDefault(p => p.panelID == panelID);

        if (entry == null)
        {
            Debug.LogWarning($"UIManager: No panel was found with the ID: {panelID.ToString()}");
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

            ApplyPanelPosition(entry.position, entry.panelData.OffSetX, entry.panelData.OffSetY);

            infoPanelObject.SetActive(true);
            currentActivePanel = infoPanelObject;

            if (entry.panelData.HowToClose == PanelClose.Time)
            {
                activeCloseCoroutine = StartCoroutine(AutoClosePanel(entry.panelData.CloseTime));
            }
        }
        else
        {
            Debug.LogError($"The panel {panelID.ToString()} does not have UIPanelData. it will be displayed empty");
        }
    }
    private IEnumerator AutoClosePanel(float delay)
    {
        yield return new WaitForSeconds(delay);
        activeCloseCoroutine = null;
        HideCurrentPanel();
    }
    private void ApplyPanelPosition(PanelPosition position, float offsetX, float offsetY)
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

            if (offsetX != 0 || offsetY != 0)
            {
                infoPanelObject.transform.position += new Vector3(offsetX, offsetY, 0);
            }
        }
    }
    public void HideCurrentPanel()
    {
        if (activeCloseCoroutine != null)
        {
            StopCoroutine(activeCloseCoroutine);
            activeCloseCoroutine = null;
        }

        if (currentActivePanel != null)
        {
            currentActivePanel.SetActive(false);
            currentActivePanel = null;
        }
    }
    #endregion
    public void PauseMenu()
    {
        bool isOpeningPause = !PauseMenuManager.Instance.PauseGameObject().activeSelf;

        if (isOpeningPause)
        {
            HideCurrentPanel();
        }

        PauseMenuManager.Instance.PauseGameObject().SetActive(isOpeningPause);
        UpdateCursorState();
    }
    public void ShowCrossHairFade(bool show)
    {
        //  crosshairFade.ShowCrosshair(show);
    }
    void UpdateCursorState()
    {
        bool shouldShowCursor = PauseMenuManager.Instance.PauseGameObject().activeInHierarchy;

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
    void PauseMenu();
    void ShowCrossHairFade(bool show);
    void ShowPanel(UIPanelID panelID);
    void HideCurrentPanel();
}