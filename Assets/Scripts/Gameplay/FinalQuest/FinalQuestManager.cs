using System;
using System.Collections.Generic;
using UnityEngine;
using Core.EventBus;
using Core.Utilities;
using Core.Data;

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
            //doorKey.gameObject.SetActive(false);
        }
      
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
           
        }
    }
#endif


}
public interface IFinalQuestManager
{
    
}