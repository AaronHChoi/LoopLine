using DependencyInjection;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MusicNotesActivations
{
    public GameCondition condition;
    public GameObject musicNote;
}

public class SafeQuestManager : MonoBehaviour
{
    [SerializeField] private int[] result, correctCombination;
    [SerializeField] SingleDoorInteract doorInteract;
    [SerializeField] ItemInteract doorKey;
    [SerializeField] List<MusicNotesActivations> musicNotesActivations;

    IInventoryUI inventoryUI;

    private void Awake()
    {
        inventoryUI = InterfaceDependencyInjector.Instance.Resolve<IInventoryUI>();
    }
    void Start()
    {
        //result = new int[] { 1, 1, 1};
        //correctCombination = new int[] { 2, 4, 3};
        if (GameManager.Instance.GetCondition(GameCondition.MusicSafeDoorOpen))
        {
            doorKey.gameObject.SetActive(false);
        }
        UpdateMusicNoteActivationStates();
    }

    private void OnEnable()
    {
        Dial.OnDialRotated += CheckResult;
        if (doorInteract != null)
        {
            doorInteract.OnPhotoQuestOpenDoor += OpenDoorMusicSafeQuest;
        }
    }
    private void OnDisable()
    {
        Dial.OnDialRotated -= CheckResult;
        if (doorInteract != null)
        {
            doorInteract.OnPhotoQuestOpenDoor -= OpenDoorMusicSafeQuest;
        }
    }

    private void CheckResult(string dialName, int indexShown)
    {
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
            Debug.Log("Safe Unlocked!");
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
    private void OpenDoorMusicSafeQuest()
    {
        inventoryUI.RemoveInventorySlot(doorKey);
        GameManager.Instance.SetCondition(GameCondition.MusicSafeDoorOpen, true);
    }


}
