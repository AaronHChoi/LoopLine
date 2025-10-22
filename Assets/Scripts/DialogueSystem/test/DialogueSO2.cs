using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue Data")]
public class DialogueSO2 : ScriptableObject
{
    [Serializable]
    public class DialogueLine
    {
        public NPCType npcType;
        [TextArea(3, 10)]
        public string dialogueText;
    }
    public bool IsAMonologue;
    public DialogueLine[] lines;

    public bool hasPostMonologue;
    public Events postMonologueEvent;
}