using Core.Data;
using Core.DependencyInjection;
using Core.EventBus;
using System;
using UnityEngine;

public class WalkManItem : ItemInteract, IWalkmanItem
{
    public event Action OnWalkManTaken;

    IUIManager uiManager;
    IGameSceneManager gameSceneManager;
    ISceneWeightController weightController;

    [SerializeField] UIPanelID panelID;

    protected override void Awake()
    {
        base.Awake();
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
        weightController = InterfaceDependencyInjector.Instance.Resolve<ISceneWeightController>();
    }
    public override void Start()
    {
        base.Start();

        if (GameManager.Instance.GetCondition(GameCondition.WalkManTaken))
        {
            this.gameObject.SetActive(false);
        }
    }
    public override bool Interact()
    {
        if (canBePicked)
        {
            OnWalkManTaken?.Invoke();
            uiManager.ShowPanel(panelID);
            GameManager.Instance.SetCondition(GameCondition.WalkManTaken, true);
            weightController.HandleConditionChanged(GameCondition.WalkManTaken, true);
            gameSceneManager.SetInitialLoop(false);
            EventBus.Publish(new PlayerGrabItemEvent());
            gameObject.SetActive(false);

            return true;
        }
        return false;
    }
}
}

public interface IWalkmanItem
{
}
