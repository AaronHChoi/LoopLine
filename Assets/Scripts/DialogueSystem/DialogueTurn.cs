using UnityEngine;

[System.Serializable]
public class DialogueTurn
{
    [field: SerializeField]public DialogueCharacterSO Character { get; private set; }

    public AudioClip sound;

    [SerializeField, TextArea(3,5)]private string dialogueLine = string.Empty;
    public string DialogueLine => dialogueLine;

}
