using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core.Utilities;
using Core.DependencyInjection;

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
    public PanelType panelType = PanelType.TutorialInfo;
}
public class UIManager : Singleton<UIManager>, IUIManager
{
    bool isCursorVisible = false;

    [Header("UI Panel Manager")]
    [SerializeField] List<UIPanelEntry> managedPanels = new List<UIPanelEntry>();
    [SerializeField] GameObject infoPanelObject;
    [SerializeField] GameObject titlePanelObject;

    [Header("Panel positions (RectTransforms)")]
    [SerializeField] Transform centerPosition;
    [SerializeField] Transform leftPosition;
    [SerializeField] Transform rightPosition;

    GameObject currentActivePanel = null;
    InfoPanel panelScript;
    InfoPanel titlePanelScript;

    Coroutine activeCloseCoroutine = null;

    ICrosshairFade crosshairFade;
    IGameStateController gameController;

    #region DEBUG_TOOLS
    [Header("Debug Settings")]
    [SerializeField] UIPanelID debugPanelToTest;

    [ContextMenu("Debug: Show Selected Panel")]
    private void DebugShowPanel()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        ShowPanel(debugPanelToTest);
    }
    [ContextMenu("DEBUG: Hide Current Panel")]
    private void DebugHidePanel()
    {
        HideCurrentPanel();
    }
    #endregion

    #region MAGIC_METHODS
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

        if (titlePanelObject != null)
        {
            titlePanelObject.SetActive(false);
            titlePanelScript = titlePanelObject.GetComponent<InfoPanel>();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
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

        GameObject targetObject = (entry.panelType == PanelType.ChapterTitle) ? titlePanelObject : infoPanelObject;
        InfoPanel targetScript = (entry.panelType == PanelType.ChapterTitle) ? titlePanelScript : panelScript;

        if (targetObject == null || targetScript == null)
        {
            Debug.LogError("The panel cannot be displayed, infoPanelObject is not setting");
            return;
        }
        
        if (entry.panelData != null)
        {
            targetScript.Setup(entry.panelData);

            ApplyPanelPosition(targetObject, entry.position, entry.panelData.OffSetX, entry.panelData.OffSetY);

            targetObject.SetActive(true);
            currentActivePanel = targetObject;

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
    private void ApplyPanelPosition(GameObject target, PanelPosition position, float offsetX, float offsetY)
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
            target.transform.position = targetPosition.position;

            if (offsetX != 0 || offsetY != 0)
            {
                target.transform.position += new Vector3(offsetX, offsetY, 0);
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "01. MainMenu")
        {
            isCursorVisible = true;
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