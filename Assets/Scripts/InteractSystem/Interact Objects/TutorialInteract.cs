using DependencyInjection;
using UnityEngine;

public class TutorialInteract : ItemInteract
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
        uiManager.ShowPanel(panelID);
        return base.Interact();
    }
}
