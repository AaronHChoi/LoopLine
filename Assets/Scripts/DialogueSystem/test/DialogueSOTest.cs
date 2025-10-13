using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/Dialogue")]
public class DialogueSOTest : ScriptableObject
{
    [System.Serializable]
    public class DialogueLine
    {
        [TextArea(3, 5)]
        public string text;
    }

    public string dialogueId;
    public Events triggerEvent;
    public DialogueLine[] dialogueLines;
}