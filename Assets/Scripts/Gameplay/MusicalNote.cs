using DependencyInjection;
using UnityEngine;

public class MusicalNote : MonoBehaviour, IInteract
{
    IClueSafeQuest clue;
    private void Awake()
    {
        clue = InterfaceDependencyInjector.Instance.Resolve<IClueSafeQuest>();
    }
    public string GetInteractText()
    {
        throw new System.NotImplementedException();
    }
    public void Interact()
    {
        if (GameManager.Instance.GetCondition(GameCondition.AllMusicNotesCollected))
        {
            clue.PlaySequence();
        }
    }
}