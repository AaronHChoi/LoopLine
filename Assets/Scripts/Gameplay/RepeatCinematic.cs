using Core.DependencyInjection;
using Core.Utilities;
using Player;
using UnityEngine;
using UnityEngine.Video;
using Core.Data;

public class RepeatCinematic : MonoBehaviour
{
    [SerializeField] VideoClip successCinematic;
    [SerializeField] GameCondition thisGameCondition;

    private bool isPlayerInside = false;

    ICinematicManager cinematicManager;
    IPlayerStateController playerStateController;
    IUIManager uiManager;

    private void Awake()
    {
        cinematicManager = InterfaceDependencyInjector.Instance.Resolve<ICinematicManager>();
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
        uiManager = InterfaceDependencyInjector.Instance.Resolve<IUIManager>();
    }
    private void Start()
    {
        if (GameManager.Instance.GetCondition(thisGameCondition))
        {
            ActivateBoxCollider(true);
        }
        else
        {
            ActivateBoxCollider(false);
        }
    }
    private void OnEnable()
    {
        playerStateController.OnActivateCinematic += RepeatCinematicAfterCompleteQuest;
    }
    private void OnDisable()
    {
        playerStateController.OnActivateCinematic -= RepeatCinematicAfterCompleteQuest;
    }
    public void ActivateBoxCollider(bool state)
    {
        this.gameObject.SetActive(state);
    }
    private void OnTriggerEnter(Collider other)
    {
        isPlayerInside = true;
        GameManager.Instance.SetCondition(GameCondition.IsCinematicActivated, true);
        uiManager.ShowPanel(UIPanelID.Cinematic);
    }
    private void OnTriggerExit(Collider other)
    {
        isPlayerInside = false;
        GameManager.Instance.SetCondition(GameCondition.IsCinematicActivated, false);
        uiManager.HideCurrentPanel();
    }
    public void RepeatCinematicAfterCompleteQuest()
    {
        if (!isPlayerInside)
        {
            return;
        }

        playerStateController.StateMachine.TransitionTo(playerStateController.CinematicState);
        DelayUtility.Instance.Delay(0.3f, () =>
        cinematicManager.PlayCinematic(successCinematic, () =>
        {
            playerStateController.StateMachine.TransitionTo(playerStateController.NormalState);
        }));
    }
}