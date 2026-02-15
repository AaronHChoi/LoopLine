using Core.Audio;
using Core.Data;
using Core.DependencyInjection;
using Core.EventBus;
using System;
using UnityEngine;

public class WalkManItem : ItemInteract, IWalkmanItem
{
    public event Action OnWalkManTaken;

    [SerializeField] GameObject ParentGameObject;
    IUIManager uiManager;
    IGameSceneManager gameSceneManager;
    ISceneWeightController weightController;
    ISoundManager soundManager;

    [SerializeField] UIPanelID panelID;

    [Header("Audio")]
    [SerializeField] SoundData soundData;

    protected override void Awake()
    {
        base.Awake();
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
        weightController = InterfaceDependencyInjector.Instance.Resolve<ISceneWeightController>();
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();
    }
    public override void Start()
    {
        base.Start();

        if (GameManager.Instance.GetCondition(GameCondition.WalkManTaken))
        {
            this.gameObject.SetActive(false);
            ParentGameObject.SetActive(false);
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
            soundManager.CreateSound()
           .WithSoundData(soundData)
           .Play();
            EventBus.Publish(new PlayerGrabItemEvent());
            gameObject.SetActive(false);
            ParentGameObject.SetActive(false);

            return true;
        }
        return false;
    }

}


public interface IWalkmanItem
{
    event Action OnWalkManTaken;
}
