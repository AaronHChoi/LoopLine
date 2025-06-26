using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Object/New Dialogue")]
public class DialogueSO : ScriptableObject
{
    [System.Serializable]
    public struct Line
    {
        public CharacterSO character;
        public AudioClip sound;
        [TextArea(3, 5)] 
        public string dialogue;
    }
    public bool Unlocked;
    public bool Finished;
    public bool ReUse;
    public bool Skipeable;
    public bool AddToWhiteboard;

    public Line[] Dialogues;
    public QuestionSO Questions;

    public void ResetValues()
    {
        Unlocked = true;
        Finished = false;
    }
}
