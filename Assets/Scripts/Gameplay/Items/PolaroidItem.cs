using System;
using UnityEngine;
using Core.Data;
using Core.DependencyInjection;
using Core.EventBus;
using Core.UI;

public class PolaroidItem : ItemInteract, IPolaraidItem
{
    public event Action OnPolaroidTaken;

    IUIManager uiManager;
    IGameSceneManager gameSceneManager;
    ISceneWeightController weightController;
    IMonologueSpeaker monologueSpeaker;

    [SerializeField] UIPanelID panelID;

    protected override void Awake()
    {
        base.Awake();
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
        weightController = InterfaceDependencyInjector.Instance.Resolve<ISceneWeightController>();
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>(MonologueID.Player);
    }
    public override void Start()
    {
        base.Start();

        if (GameManager.Instance.GetCondition(GameCondition.PolaroidTaken))
        {
            this.gameObject.SetActive(false);
        }
    }
    public override bool Interact()
    {
        if (canBePicked)
        {
            OnPolaroidTaken?.Invoke();
            uiManager.ShowPanel(panelID);
            GameManager.Instance.SetCondition(GameCondition.PolaroidTaken, true);
            weightController.HandleConditionChanged(GameCondition.PolaroidTaken, true);
            gameSceneManager.SetInitialLoop(false);
            EventBus.Publish(new PlayerGrabItemEvent());
            gameObject.SetActive(false);
            monologueSpeaker.StartMonologue(Events.PolaroidTakenMonologue);

            return true;
        }
        return false;
    }
}
public interface IPolaraidItem
{
    event Action OnPolaroidTaken;
}