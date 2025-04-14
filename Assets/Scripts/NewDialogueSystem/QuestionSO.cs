using UnityEngine;

[System.Serializable]
public struct Options
{
    [TextArea(2, 4)]
    public string option;
    public DialogueSO dialogue;
}

[CreateAssetMenu(fileName = "Question", menuName = "Scriptable Object/New Question")]
public class QuestionSO : ScriptableObject
{
    [TextArea(3, 5)]
    public string Question;
    public CharacterSO CharacterName;
    public Options[] Options;
}