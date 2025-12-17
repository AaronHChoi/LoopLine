using DependencyInjection;
using Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[Serializable]
public struct MusicNotesActivations
{
    public GameCondition condition;
    public GameObject musicNote;
}
public class SafeQuestManager : MonoBehaviour, ISafeQuestManager
{
    [SerializeField] private int[] result, correctCombination;
    [SerializeField] SingleDoorInteract doorInteract;
    [SerializeField] ItemInteract doorKey;
    [SerializeField] List<MusicNotesActivations> musicNotesActivations;

    [SerializeField] VideoClip successCinematic;

    public event Action OnSafeQuestCompleted;

    bool isSolved = false;

    IInventoryUI inventoryUI;
    IFinalQuestManager finalQuestManager;
    ICinematicManager cinematicManager;
    IPlayerStateController playerStateController;
    private void Awake()
    {
        inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
        finalQuestManager = InterfaceDependencyInjector.Instance.Resolve<IFinalQuestManager>();
        cinematicManager = InterfaceDependencyInjector.Instance.Resolve<ICinematicManager>();
        playerStateController = InterfaceDependencyInjector.Instance.Resolve<IPlayerStateController>();
    }
    void Start()
    {
        //result = new int[] { 1, 1, 1};
        //correctCombination = new int[] { 2, 4, 3};
        if (GameManager.Instance.GetCondition(GameCondition.IsMusicQuestComplete))
        {
            isSolved = true;
        }
        if (GameManager.Instance.GetCondition(GameCondition.MusicSafeDoorOpen))
        {
            doorKey.gameObject.SetActive(false);
        }
        UpdateMusicNoteActivationStates();
    }

    private void OnEnable()
    {
        Dial.OnDialRotated += CheckResult;
    }
    private void OnDisable()
    {
        Dial.OnDialRotated -= CheckResult;
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameManager.Instance.SetCondition(GameCondition.WordGroup3, true);
            GameManager.Instance.SetCondition(GameCondition.MusicNote1, true);
            GameManager.Instance.SetCondition(GameCondition.MusicNote2, true);
            GameManager.Instance.SetCondition(GameCondition.MusicNote3, true);
            GameManager.Instance.SetCondition(GameCondition.MusicNote4, true);
        }
    }
#endif
    private void CheckResult(string dialName, int indexShown)
    {
        if (isSolved)
        {
            return;
        }

        switch (dialName)
        {
            case "Dial1":
                result[0] = indexShown;
                break;
            case "Dial2":
                result[1] = indexShown;
                break;
            case "Dial3":
                result[2] = indexShown;
                break;
            case "Dial4":
                result[3] = indexShown;
                break;
        }
        if (result[0] == correctCombination[0] &&
            result[1] == correctCombination[1] &&
            result[2] == correctCombination[2] &&
            result[3] == correctCombination[3])
        {
            isSolved = true;

            playerStateController.StateMachine.TransitionTo(playerStateController.CinematicState);
            DelayUtility.Instance.Delay(1f, () =>
                cinematicManager.PlayCinematic(successCinematic, () =>
                {
                    OnSafeQuestCompleted?.Invoke();

                    GameManager.Instance.SetCondition(GameCondition.IsMusicQuestComplete, true);

                    GameManager.Instance.SetCondition(GameCondition.WordGroup3, true);

                    finalQuestManager.UpdateWordsActivation();

                    playerStateController.StateMachine.TransitionTo(playerStateController.NormalState);
                })
            );
        }
    }
    void UpdateMusicNoteActivationStates()
    {
        if (musicNotesActivations == null) return;

        foreach (var entry in musicNotesActivations)
        {
            bool isConditionMet = GameManager.Instance.GetCondition(entry.condition);

            if (entry.musicNote != null)
            {
                entry.musicNote.SetActive(isConditionMet);
            }
        }
    }
    public void OpenDoorMusicSafeQuest()
    {
        inventoryUI.RemoveInventorySlot(doorKey);
        GameManager.Instance.SetCondition(GameCondition.MusicSafeDoorOpen, true);
    }
}

public interface ISafeQuestManager
{
    event Action OnSafeQuestCompleted;
    void OpenDoorMusicSafeQuest();
}