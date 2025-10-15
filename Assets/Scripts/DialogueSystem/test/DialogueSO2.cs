using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue Data")]
public class DialogueSO2 : ScriptableObject
{
    public bool IsAMonologue;

    [TextArea(3, 10)]
    public string dialogueText;
}