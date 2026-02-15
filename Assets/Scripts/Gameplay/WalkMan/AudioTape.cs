using Core.Audio;
using Core.Data;
using Core.DependencyInjection;
using Core.EventBus;
using UnityEngine;
using Core.Utilities;
using Player;


public class AudioTape : ItemInteract, IAudioTape
{
    [Header("Audio")]
    [SerializeField] SoundData soundData;

    [SerializeField] private Events monologueToTrigger;
    [SerializeField] private GameCondition conditionToTrigger;

    [SerializeField] public GameObject parentGameObject;

    IWalkman walkman;
    IPlayerStateController stateController;

    [SerializeField] UIPanelID panelID;


    protected override void Awake()
    {
        base.Awake();
        walkman = InterfaceDependencyInjector.Instance.Resolve<IWalkman>();
        stateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
    }

    public override bool Interact()
    {
        if (stateController.StateMachine.CurrentState == stateController.WalkManMusicState)
        {
            walkman.HandleListenMusic(parentGameObject);

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


