using UnityEngine;

[CreateAssetMenu(fileName ="New Dialogue Character", menuName ="Scriptable Objects/Dialogue Character")]
public class DialogueCharacterSO : ScriptableObject
{
    [Header("Character Info")]
    [SerializeField] private string characterName;

    public string Name => characterName;
}
