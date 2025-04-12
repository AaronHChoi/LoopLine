using UnityEngine;
using System.Collections.Generic;
using Unity.Multiplayer.Center.Common;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Scriptable Objects/Dialogue Round")]
public class DialogueRoundSO : ScriptableObject
{
    [SerializeField] private List<DialogueTurn> dialogueTurnsList;
    public List<DialogueTurn> DialogueTurnsList => dialogueTurnsList;

    public bool Unlocked;
    public bool Finished;
    public bool ReUse;
    
    public DialogueQuestionSO question;
}
