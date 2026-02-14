using Core.Audio;
using Core.Data;
using Core.DependencyInjection;
using Core.EventBus;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AudioTape : ItemInteract, IAudioTape
{
    [Header("Audio")]
    [SerializeField] SoundData soundData;

    [SerializeField] private Events monologueToTrigger;
    [SerializeField] private GameCondition conditionToTrigger;


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

    public override bool Interact()
    {
        if (canBePicked)
        {
            GameManager.Instance.SetCondition(conditionToTrigger, true);
            weightController.HandleConditionChanged(conditionToTrigger, true);
            gameSceneManager.SetInitialLoop(false);
            EventBus.Publish(new PlayerGrabItemEvent());
            gameObject.SetActive(false);

            return true;
        }
        return false;
    }

    public SoundData GetSoundData()
    {
        return soundData;
    }

    public Events GetMonologueToTrigger()
    {
        return monologueToTrigger;
    }
}
public interface IAudioTape
{
    Events GetMonologueToTrigger();
    SoundData GetSoundData();
}


