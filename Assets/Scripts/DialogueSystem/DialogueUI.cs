using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private RectTransform dialogueBox;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI dialogueArea;
    public void ShowDialogueBox()
    {
        dialogueBox.gameObject.SetActive(true);
    }
    public void HideDialogueBox()
    {
        dialogueBox.gameObject.SetActive(false);
    }
    public void SetCharacterInfo(DialogueCharacterSO character)
    {
        if (character == null) return;
        characterName.text = character.Name;
    }
    public void ClearDialogueArea()
    {
        dialogueArea.text = string.Empty;
    }
    public void SetDialogueArea(string text)
    {
        dialogueArea.text = text;
    }
    public void AppendToDialogueArea(char letter)
    {
        dialogueArea.text += letter;
    }
}
