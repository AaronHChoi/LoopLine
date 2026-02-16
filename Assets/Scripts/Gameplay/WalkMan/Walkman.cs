using Core.Audio;
using Core.DependencyInjection;
using Core.UI;
using Core.Utilities;
using Player;
using System.Collections;
using UnityEngine;

public class Walkman : MonoBehaviour, IWalkman
{

    [Header("WalkMan")]
    [SerializeField] GameObject walkmanUI;
    [SerializeField] float delay;
    [SerializeField] GameObject UICassette;
    [SerializeField] public bool isListeningAudioTape { get; set; }

    [Header("Audio")]
    [SerializeField] SoundData soundData;
    [SerializeField] SoundData soundData2;


    IPlayerStateController playerStateController;
    ICarretteController carretteControllerLeft;
    ICarretteController carretteControllerRight;
    IPlayerInteract playerInteract;
    IMonologueSpeaker monologueSpeaker;
    ISoundManager soundManager;


    private void Awake()
    {
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        carretteControllerLeft = InterfaceDependencyInjector.Instance.Resolve<ICarretteController>(AnimatorEnum.UI_Carrette_Left);
        carretteControllerRight = InterfaceDependencyInjector.Instance.Resolve<ICarretteController>(AnimatorEnum.UI_Carrette_Right);
        playerInteract = InterfaceDependencyInjector.Instance.Resolve<IPlayerInteract>();
        monologueSpeaker = InterfaceDependencyInjector.Instance.Resolve<IMonologueSpeaker>();
        soundManager = InterfaceDependencyInjector.Instance.Resolve<ISoundManager>();
    }


    public void HandleListenMusic(GameObject parentGameObject)
    {
        if (isListeningAudioTape) return;

        StartCoroutine(ListenAudioTape(parentGameObject));
    }

    IEnumerator ListenAudioTape(GameObject parentGameObject)
    {
        yield return new WaitForEndOfFrame();

        //uiAnimation.PhotoUIAnimation();

        //yield return new WaitForSeconds(delay);

        GameObject target = playerInteract.GetRaycastTarget();

        if (target != null && target.TryGetComponent(out IAudioTape tapeTarget))
        {
            //target.gameObject.SetActive(false);
            //parentGameObject.SetActive(false);

            soundManager.CreateSound()
           .WithSoundData(tapeTarget.GetSoundData())
           .Play();

            isListeningAudioTape = true;
            carretteControllerLeft.SetRotation(true);
            carretteControllerRight.SetRotation(true);

            monologueSpeaker.StartMonologue(tapeTarget.GetMonologueToTrigger());

            yield return new WaitForSeconds(tapeTarget.GetSoundData().clip.length);

            carretteControllerLeft.SetRotation(false);
            carretteControllerRight.SetRotation(false);

            isListeningAudioTape = false;
            //target.gameObject.SetActive(true);
            //parentGameObject.SetActive(true);

        }

    }

    public void SetWalkManUIVisible(bool isVisible)
    {
        walkmanUI.SetActive(isVisible);
        RectTransform rect = UICassette.GetComponent<RectTransform>();

        rect.anchoredPosition = new Vector2(0, -45);

        
    }
}
public interface IWalkman
{
    bool isListeningAudioTape { get; set; }
    void SetWalkManUIVisible(bool isVisible);
    void HandleListenMusic(GameObject parentGameObject);
}
