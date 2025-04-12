using UnityEngine;

[System.Serializable]
public struct Options
{
    [TextArea(2, 4)]
    public string Option;
    public DialogueRoundSO dialogueRound;
}

[CreateAssetMenu(fileName = "New Question", menuName = "Scriptable Objects/Dialogue Question")]
public class DialogueQuestionSO : ScriptableObject
{
    [TextArea(3, 5)]
    public string Question;
    public Options[] Options;

}
