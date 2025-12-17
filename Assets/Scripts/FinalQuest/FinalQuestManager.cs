using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct WordsActivations
{
    public GameCondition condition;
    public GameObject musicNote;
}
public class FinalQuestManager : MonoBehaviour, IFinalQuestManager
{
    [SerializeField] private int[] result, correctCombination;
    [SerializeField] SingleDoorInteract doorInteract;
    [SerializeField] ItemInteract doorKey;
    [SerializeField] GameObject FinalQuestGameObject;
    [SerializeField] List<WordsActivations> wordsActivations;

    public event Action OnQuestCompleted;

    void Start()
    {
        //result = new int[] { 1, 1, 1};
        //correctCombination = new int[] { 2, 4, 3};
        if (GameManager.Instance.GetCondition(GameCondition.MusicSafeDoorOpen))
        {
            doorKey.gameObject.SetActive(false);
        }
        UpdateWordsActivation();
    }
    private void OnEnable()
    {
        FinalQuestDial.OnDialRotated += CheckResult;
    }
    private void OnDisable()
    {
        FinalQuestDial.OnDialRotated -= CheckResult;
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            UpdateWordsActivation();
        }
    }
#endif

    private void CheckResult(string dialName, int indexShown)
    {
        switch (dialName)
        {
            case "WordGroup1":
                result[0] = indexShown;
                break;
            case "WordGroup2":
                result[1] = indexShown;
                break;
            case "WordGroup3":
                result[2] = indexShown;
                break;
        }
        if (result[0] == correctCombination[0] &&
            result[1] == correctCombination[1] &&
            result[2] == correctCombination[2] )
        {
            DelayUtility.Instance.Delay(5f, () =>
            {
                GameManager.Instance.SetCondition(GameCondition.FinalQuestCompleted, true);

                EventBus.Publish(new FinalQuestCompleteEvent());
                DelayUtility.Instance.Delay(2f, () => FinalQuestGameObject.gameObject.SetActive(false));
                OnQuestCompleted?.Invoke();
            });
        }
    }
    public void UpdateWordsActivation()
    {
        if (wordsActivations == null) return;

        foreach (var entry in wordsActivations)
        {
            bool isConditionMet = GameManager.Instance.GetCondition(entry.condition);

            if (entry.musicNote != null)
            {
                entry.musicNote.SetActive(isConditionMet);
            }
        }
    }
}
public interface IFinalQuestManager
{
    void UpdateWordsActivation();
    event Action OnQuestCompleted;
}