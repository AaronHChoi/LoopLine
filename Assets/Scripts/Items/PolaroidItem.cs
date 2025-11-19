
using System;
using DependencyInjection;
using UnityEngine;

public class PolaroidItem : ItemInteract, IPolaraidItem
{
    public event Action OnPolaroidTaken;

    IUIManager uiManager;
    IGameSceneManager gameSceneManager;
    [SerializeField] UIPanelID panelID;

    protected override void Awake()
    {
        base.Awake();
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
    }
    public override void Start()
    {
        base.Start();
    }
    public override bool Interact()
    {
        if (canBePicked)
        {
            OnPolaroidTaken?.Invoke();
            uiManager.ShowPanel(panelID);
            GameManager.Instance.SetCondition(GameCondition.PolaroidTaken, true);
            gameSceneManager.SetInitialLoop(false);

            gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}
public interface IPolaraidItem
{
    event Action OnPolaroidTaken;
}