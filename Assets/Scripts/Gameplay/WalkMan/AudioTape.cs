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
    [SerializeField] private GameObject parentGameObject;

    IUIManager uiManager;
    IGameSceneManager gameSceneManager;
    ISceneWeightController weightController;
    ISoundManager soundManager;
    IWalkman walkman;
    IMonologueSpeaker monologueSpeaker;
    IPlayerStateController stateController;

    [SerializeField] UIPanelID panelID;


    protected override void Awake()
    {
        base.Awake();
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
        gameSceneManager = InterfaceDependencyInjector.Instance.Resolve<IGameSceneManager>();
        weightController = InterfaceDependencyInjector.Instance.Resolve<ISceneWeightController>();
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();
        walkman = InterfaceDependencyInjector.Instance.Resolve<IWalkman>();
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
        stateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
    }

    public override bool Interact()
    {
        if (!canBePicked)
        {
           // gameObject.SetActive(false);
           // parentGameObject.SetActive(false);

           // soundManager.CreateSound()
           //.WithSoundData(soundData)
           //.Play();

           // walkman.isListeningAudioTape = true;
           // monologueSpeaker.StartMonologue(monologueToTrigger);

           // DelayUtility.Instance.Delay(soundData.clip.length, () => 
           // { 
           //     walkman.isListeningAudioTape = false;
           //     gameObject.SetActive(true);
           // });
            walkman.HandleListenMusic();

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


