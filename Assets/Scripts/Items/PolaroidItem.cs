
using DependencyInjection;
using UnityEngine;

public class PolaroidItem : ItemInteract
{
    IUIManager uiManager;
    [SerializeField] UIPanelID panelID;
    protected override void Awake()
    {
        base.Awake();
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
    }
    public override void Start()
    {
        base.Start();
    }
    public override bool Interact()
    {
        if (canBePicked)
        {
            GameManager.Instance.HasCamera = true;
            uiManager.ShowPanel(panelID);
            GameManager.Instance.SetCondition(GameCondition.PolaroidTaken, true);
            gameObject.SetActive(false);
            return true;
        }
        return false;
    }


}